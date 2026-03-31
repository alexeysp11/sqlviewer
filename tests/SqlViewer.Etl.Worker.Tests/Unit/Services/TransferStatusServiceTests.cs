using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Etl.Worker.Services;
using SqlViewer.Shared.Messages.Etl.Events;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Worker.Tests.Unit.Services;

public sealed class TransferStatusServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<ILogger<TransferStatusService>> _loggerMock = new();

    [Fact]
    public async Task ProcessAsync_WhenJobExists_ShouldUpdateStatusAndAddLog()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        using EtlDbContext db = new(options);
        Guid correlationId = Guid.NewGuid();

        // 1. Create a job that should be updated
        TransferJobEntity job = _fixture.Build<TransferJobEntity>()
            .With(j => j.CorrelationId, correlationId)
            .With(j => j.CurrentStatus, TransferStatus.Queued)
            .Create();
        db.TransferJobs.Add(job);
        await db.SaveChangesAsync();

        // 2. Prepare the incoming message
        DataTransferStatusUpdated payload = new(
            MessageId: _fixture.Create<Guid>(),
            CorrelationId: correlationId,
            TransferStatus: TransferStatus.Progress.ToString(),
            InternalStatus: "In Progress",
            Timestamp: DateTime.UtcNow,
            ErrorMessage: null
        );

        InboxMessageEntity message = _fixture.Build<InboxMessageEntity>()
            .With(m => m.CorrelationId, correlationId)
            .With(m => m.Payload, JsonSerializer.Serialize(payload))
            .Create();

        TransferStatusService handler = new(db, _loggerMock.Object);

        // Act
        await handler.ProcessAsync(message, CancellationToken.None);

        // Assert
        // Check job status update
        TransferJobEntity? updatedJob = await db.TransferJobs.FirstAsync(j => j.CorrelationId == correlationId);
        updatedJob.CurrentStatus.Should().Be(TransferStatus.Progress);

        // Check log entry creation
        TransferStatusLogEntity? log = await db.TransferStatusLogs
            .FirstOrDefaultAsync(l => l.CorrelationId == correlationId && l.Status == TransferStatus.Progress);

        log.Should().NotBeNull();
        log!.MetadataJson.Should().Be(message.Payload);
        log.Timestamp.Should().BeCloseTo(payload.Timestamp, TimeSpan.FromMilliseconds(100));
    }

    [Fact]
    public async Task ProcessAsync_WhenJobNotFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        using EtlDbContext db = new(options);
        TransferStatusService handler = new(db, _loggerMock.Object);

        DataTransferStatusUpdated payload = _fixture.Create<DataTransferStatusUpdated>();
        InboxMessageEntity message = _fixture.Build<InboxMessageEntity>()
            .With(m => m.Payload, JsonSerializer.Serialize(payload))
            .Create();

        // Act
        Func<Task> act = async () => await handler.ProcessAsync(message, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"TransferJob with CorrelationId {message.CorrelationId} not found.");
    }

    [Fact]
    public async Task ProcessAsync_WithInvalidPayload_ShouldThrowJsonException()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        using EtlDbContext db = new(options);
        TransferStatusService handler = new(db, _loggerMock.Object);

        InboxMessageEntity message = _fixture.Build<InboxMessageEntity>()
            .With(m => m.Payload, "invalid-json")
            .Create();

        // Act
        Func<Task> act = async () => await handler.ProcessAsync(message, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<JsonException>();
    }

    [Fact]
    public async Task ProcessAsync_ShouldNotUpdateJobStatus_WhenIncomingStatusHasLowerRank()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using EtlDbContext db = new(options);
        TransferStatusService handler = new(db, _loggerMock.Object);

        Guid correlationId = Guid.NewGuid();

        // Initial state: Job is already Completed
        TransferJobEntity job = _fixture.Build<TransferJobEntity>()
            .With(j => j.CorrelationId, correlationId)
            .With(j => j.CurrentStatus, TransferStatus.Completed)
            .Create();

        db.TransferJobs.Add(job);
        await db.SaveChangesAsync();

        // Incoming message: Progress (lower rank than Completed)
        DataTransferStatusUpdated payload = _fixture.Build<DataTransferStatusUpdated>()
            .With(x => x.TransferStatus, TransferStatus.Progress.ToString())
            .With(x => x.Timestamp, DateTime.UtcNow)
            .Create();

        InboxMessageEntity message = _fixture.Build<InboxMessageEntity>()
            .With(m => m.CorrelationId, correlationId)
            .With(m => m.Payload, JsonSerializer.Serialize(payload))
            .Create();

        // Act
        await handler.ProcessAsync(message, CancellationToken.None);

        // Assert
        TransferJobEntity updatedJob = await db.TransferJobs.FirstAsync(j => j.CorrelationId == correlationId);

        // Job status should remain Completed
        updatedJob.CurrentStatus.Should().Be(TransferStatus.Completed);

        // But log must be added anyway
        db.TransferStatusLogs.Should().ContainSingle(l =>
            l.CorrelationId == correlationId && l.Status == TransferStatus.Progress);
    }

    [Fact]
    public async Task ProcessAsync_ShouldHandleOutOfOrderMessages_MaintainingHighestRank()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using EtlDbContext db = new(options);
        TransferStatusService handler = new(db, _loggerMock.Object);

        Guid correlationId = Guid.NewGuid();
        TransferJobEntity job = _fixture.Build<TransferJobEntity>()
            .With(j => j.CorrelationId, correlationId)
            .With(j => j.CurrentStatus, TransferStatus.Queued)
            .Create();

        db.TransferJobs.Add(job);
        await db.SaveChangesAsync();

        // Prepare two messages: Progress and Completed
        string completedPayload = JsonSerializer.Serialize(_fixture.Build<DataTransferStatusUpdated>()
            .With(x => x.TransferStatus, TransferStatus.Completed.ToString())
            .Create());
        string progressPayload = JsonSerializer.Serialize(_fixture.Build<DataTransferStatusUpdated>()
            .With(x => x.TransferStatus, TransferStatus.Progress.ToString())
            .Create());

        InboxMessageEntity msgCompleted = _fixture.Build<InboxMessageEntity>()
            .With(m => m.CorrelationId, correlationId)
            .With(m => m.Payload, completedPayload).Create();

        InboxMessageEntity msgProgress = _fixture.Build<InboxMessageEntity>()
            .With(m => m.CorrelationId, correlationId)
            .With(m => m.Payload, progressPayload).Create();

        // Act: Process Completed first, then Progress (Out of order)
        await handler.ProcessAsync(msgCompleted, CancellationToken.None);
        await handler.ProcessAsync(msgProgress, CancellationToken.None);

        // Assert
        TransferJobEntity finalJob = await db.TransferJobs.FirstAsync(j => j.CorrelationId == correlationId);

        // Final status must be Completed
        finalJob.CurrentStatus.Should().Be(TransferStatus.Completed);

        // Verify both logs exist
        List<TransferStatusLogEntity> logs = await db.TransferStatusLogs
            .Where(l => l.CorrelationId == correlationId)
            .ToListAsync();

        logs.Should().HaveCount(2);
        logs.Should().Contain(l => l.Status == TransferStatus.Completed);
        logs.Should().Contain(l => l.Status == TransferStatus.Progress);
    }
}
