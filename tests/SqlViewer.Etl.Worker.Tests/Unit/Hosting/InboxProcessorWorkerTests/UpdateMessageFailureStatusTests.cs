using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Worker.Hosting;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.Etl.Worker.Tests.Unit.Hosting.InboxProcessorWorkerTests;

public sealed class UpdateMessageFailureStatusTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();

    [Fact]
    public async Task UpdateMessageFailureStatus_WhenCalled_ShouldIncrementRetryCountAndSetError()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using EtlDbContext db = new(options);

        InboxMessageEntity message = _fixture.Build<InboxMessageEntity>()
            .With(m => m.RetryCount, 0)
            .With(m => m.Status, InboxStatus.Received).Create();
        db.InboxMessages.Add(message);
        await db.SaveChangesAsync();

        SetupScopeFactory(db);
        InboxProcessorWorker worker = CreateWorker();
        string errorMessage = "Test Error";

        // Act
        // Using reflection or making method internal/public for testing
        await worker.UpdateMessageFailureStatus(message.Id, errorMessage, CancellationToken.None);

        // Assert
        InboxMessageEntity? result = await db.InboxMessages.FindAsync(message.Id);
        result.Should().NotBeNull();
        result!.RetryCount.Should().Be(1);
        result.Error.Should().Be(errorMessage);
        result.Status.Should().Be(InboxStatus.Received);
    }

    [Fact]
    public async Task UpdateMessageFailureStatus_WhenMaxRetriesReached_ShouldSetStatusToFailed()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using EtlDbContext db = new(options);

        InboxMessageEntity message = _fixture.Build<InboxMessageEntity>()
            .With(m => m.RetryCount, 4) // MaxRetryCount is 5
            .Create();
        db.InboxMessages.Add(message);
        await db.SaveChangesAsync();

        SetupScopeFactory(db);
        InboxProcessorWorker worker = CreateWorker();

        // Act
        await worker.UpdateMessageFailureStatus(message.Id, "Final Error", CancellationToken.None);

        // Assert
        InboxMessageEntity? result = await db.InboxMessages.FindAsync(message.Id);
        result!.Status.Should().Be(InboxStatus.Failed);
    }

    private void SetupScopeFactory(EtlDbContext db)
    {
        Mock<IServiceScope> scopeMock = new();
        scopeMock.Setup(s => s.ServiceProvider.GetService(typeof(EtlDbContext))).Returns(db);
        _scopeFactoryMock.Setup(s => s.CreateScope()).Returns(scopeMock.Object);
    }

    private InboxProcessorWorker CreateWorker() =>
        new(_scopeFactoryMock.Object, Mock.Of<ILogger<InboxProcessorWorker>>());
}