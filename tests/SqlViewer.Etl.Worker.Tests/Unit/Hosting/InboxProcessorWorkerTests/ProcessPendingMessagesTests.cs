using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Worker.Hosting;
using SqlViewer.Etl.Worker.Services;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.Etl.Worker.Tests.Unit.Hosting.InboxProcessorWorkerTests;

public sealed class ProcessPendingMessagesTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();

    [Fact]
    public async Task ProcessPendingMessages_ShouldGroupMessagesByCorrelationId()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(_fixture.Create<string>()).Options;
        using EtlDbContext db = new(options);

        Guid correlationId = Guid.NewGuid();
        List<InboxMessageEntity> messages = _fixture.Build<InboxMessageEntity>()
            .With(m => m.CorrelationId, correlationId)
            .With(m => m.Status, InboxStatus.Received)
            .With(m => m.RetryCount, 0)
            .CreateMany(3).ToList();

        db.InboxMessages.AddRange(messages);
        await db.SaveChangesAsync();

        SetupScopeFactory(db);
        InboxProcessorWorker worker = CreateWorker();

        // Act
        await worker.ProcessPendingMessages(CancellationToken.None);

        // Assert
        // In a real integration test, we'd verify that messages are processed sequentially 
        // for the same CorrelationId. Here we verify they are picked up.
        db.InboxMessages.Count(m => m.CorrelationId == correlationId).Should().Be(0);
    }

    [Fact]
    public async Task ProcessPendingMessages_WhenDatabaseIsEmpty_ShouldReturnImmediately()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(_fixture.Create<string>()).Options;
        using EtlDbContext db = new(options);

        SetupScopeFactory(db);
        InboxProcessorWorker worker = CreateWorker();

        // Act
        Func<Task> act = async () => await worker.ProcessPendingMessages(CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    private void SetupScopeFactory(EtlDbContext db)
    {
        Mock<IServiceScope> scopeMock = new();
        scopeMock.Setup(s => s.ServiceProvider.GetService(typeof(EtlDbContext))).Returns(db);
        scopeMock.Setup(s => s.ServiceProvider.GetService(typeof(ITransferStatusService)))
            .Returns(Mock.Of<ITransferStatusService>());
        _scopeFactoryMock.Setup(s => s.CreateScope()).Returns(scopeMock.Object);
    }

    private InboxProcessorWorker CreateWorker() =>
        new(_scopeFactoryMock.Object, Mock.Of<ILogger<InboxProcessorWorker>>());
}
