using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Enums;
using SqlViewer.DataTransfer.Worker.Hosting;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Hosting;

public sealed class SagaTimeoutWorkerTests : IDisposable
{
    private readonly DataTransferDbContext _db;
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<ILogger<SagaTimeoutWorker>> _loggerMock = new();
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Fixture _fixture = new();
    private readonly SagaTimeoutWorker _worker;

    public SagaTimeoutWorkerTests()
    {
        // In-Memory database setup
        DbContextOptions<DataTransferDbContext> options = new DbContextOptionsBuilder<DataTransferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new DataTransferDbContext(options);

        // Configuration setup for Kafka topic
        _configMock.Setup(c => c[ConfigurationKeys.Services.Kafka.Topics.DataTransferStatusUpdates]).Returns(_fixture.Create<string>());

        // DI Scope setup
        Mock<IServiceScope> scopeMock = new();
        Mock<IServiceProvider> serviceProviderMock = new();

        _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
        scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(DataTransferDbContext))).Returns(_db);
        serviceProviderMock.Setup(x => x.GetService(typeof(IConfiguration))).Returns(_configMock.Object);

        _worker = new SagaTimeoutWorker(_scopeFactoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ProcessStalledMessages_ShouldUpdateStalledSaga_And_CreateOutboxMessage()
    {
        // Arrange
        // Create a saga that updated 61 minutes ago (exceeding the 60 min threshold)
        DateTime lastUpdate = DateTime.UtcNow.AddMinutes(-61);
        DataTransferSagaEntity stalledSaga = _fixture.Build<DataTransferSagaEntity>()
            .With(s => s.CurrentState, TransferSagaStatus.Transferring)
            .With(s => s.LastUpdatedAt, lastUpdate)
            .With(s => s.UserUid, Guid.NewGuid().ToString())
            .Create();

        // Create a healthy saga that updated 5 minutes ago
        DataTransferSagaEntity healthySaga = _fixture.Build<DataTransferSagaEntity>()
            .With(s => s.CurrentState, TransferSagaStatus.Transferring)
            .With(s => s.LastUpdatedAt, DateTime.UtcNow.AddMinutes(-5))
            .Create();

        _db.DataTransferSagas.AddRange(stalledSaga, healthySaga);
        await _db.SaveChangesAsync();

        // Act
        await _worker.ProcessStalledMessages(CancellationToken.None);

        // Assert
        // 1. Check if stalled saga state is updated
        DataTransferSagaEntity updatedSaga = await _db.DataTransferSagas
            .FirstAsync(s => s.CorrelationId == stalledSaga.CorrelationId);
        updatedSaga.CurrentState.Should().Be(TransferSagaStatus.TimedOut);

        // 2. Check if healthy saga remains untouched
        DataTransferSagaEntity ignoredSaga = await _db.DataTransferSagas
            .FirstAsync(s => s.CorrelationId == healthySaga.CorrelationId);
        ignoredSaga.CurrentState.Should().Be(TransferSagaStatus.Transferring);

        // 3. Check Audit Log entry
        TransferExecutionEntity? auditLog = await _db.TransferExecutions
            .FirstOrDefaultAsync(e => e.CorrelationId == stalledSaga.CorrelationId && e.Status == TransferExecutionStatus.TimedOut);
        auditLog.Should().NotBeNull();

        // 4. Check Outbox message for Kafka notification
        OutboxMessageEntity? outboxMessage = await _db.OutboxMessages
            .FirstOrDefaultAsync(m => m.CorrelationId == stalledSaga.CorrelationId);

        outboxMessage.Should().NotBeNull();
        outboxMessage!.MessageType.Should().Be("DataTransferStatusUpdated");
        outboxMessage.Payload.Should().Contain(TransferSagaStatus.TimedOut.ToString());
    }

    [Fact]
    public async Task ProcessStalledMessages_ShouldSetUserUidToNull_WhenSagaHasInvalidUserUid()
    {
        // Arrange
        DataTransferSagaEntity sagaWithInvalidUid = _fixture.Build<DataTransferSagaEntity>()
            .With(s => s.CurrentState, TransferSagaStatus.Transferring)
            .With(s => s.LastUpdatedAt, DateTime.UtcNow.AddMinutes(-61))
            .With(s => s.UserUid, "not-a-guid-value")
            .Create();

        _db.DataTransferSagas.Add(sagaWithInvalidUid);
        await _db.SaveChangesAsync();

        // Act
        await _worker.ProcessStalledMessages(CancellationToken.None);

        // Assert
        OutboxMessageEntity? outboxMessage = await _db.OutboxMessages
            .FirstOrDefaultAsync(m => m.CorrelationId == sagaWithInvalidUid.CorrelationId);

        outboxMessage.Should().NotBeNull();
        outboxMessage!.UserUid.Should().BeNull();
    }

    [Fact]
    public async Task ProcessStalledMessages_ShouldUpdateOrphanExecutions_WhenNoSagaAssociated()
    {
        // Arrange
        DateTime deadline = DateTime.UtcNow.AddMinutes(-61);

        // Create an orphan execution (status Transferring, but no active saga processing it)
        TransferExecutionEntity orphanExecution = new()
        {
            CorrelationId = Guid.NewGuid(),
            Status = TransferExecutionStatus.Transferring,
            TableName = "OrphanTable",
            Timestamp = deadline
        };

        _db.TransferExecutions.Add(orphanExecution);
        await _db.SaveChangesAsync();

        // Act
        await _worker.ProcessStalledMessages(CancellationToken.None);

        // Assert
        TransferExecutionEntity updatedExecution = await _db.TransferExecutions
            .FirstAsync(e => e.CorrelationId == orphanExecution.CorrelationId);

        updatedExecution.Status.Should().Be(TransferExecutionStatus.TimedOut);
        updatedExecution.LastErrorMessage.Should().Be($"Terminated by {nameof(SagaTimeoutWorker)} due to inactivity.");
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
