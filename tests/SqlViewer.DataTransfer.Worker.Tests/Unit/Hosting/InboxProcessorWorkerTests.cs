using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Hosting;
using SqlViewer.DataTransfer.Worker.Sagas;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Hosting;

public sealed class InboxProcessorWorkerTests : IDisposable
{
    private readonly DataTransferDbContext _db;
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<ILogger<InboxProcessorWorker>> _loggerMock = new();
    private readonly Mock<IDataTransferSagaOrchestrator> _orchestratorMock;
    private readonly InboxProcessorWorker _worker;
    private readonly Fixture _fixture = new();

    public InboxProcessorWorkerTests()
    {
        // In-Memory DB
        DbContextOptions<DataTransferDbContext> options = new DbContextOptionsBuilder<DataTransferDbContext>()
            .UseInMemoryDatabase(_fixture.Create<Guid>().ToString())
            .Options;
        _db = new DataTransferDbContext(options);

        // Setting up an orchestrator mock
        _orchestratorMock = new Mock<IDataTransferSagaOrchestrator>();

        // DI Scope
        Mock<IServiceScope> scopeMock = new();
        Mock<IServiceProvider> serviceProviderMock = new();
        _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
        scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(DataTransferDbContext))).Returns(_db);
        serviceProviderMock.Setup(x => x.GetService(typeof(IDataTransferSagaOrchestrator))).Returns(_orchestratorMock.Object);

        _worker = new InboxProcessorWorker(_loggerMock.Object, _scopeFactoryMock.Object);
    }

    [Fact]
    public async Task ProcessPendingMessagesAsync_ShouldOnlyPickReceivedMessages()
    {
        // Arrange
        Guid receivedId = _fixture.Create<Guid>();
        Guid processingId = _fixture.Create<Guid>();

        _db.InboxMessages.AddRange(
            new InboxMessageEntity
            {
                MessageId = receivedId,
                CorrelationId = receivedId,
                Status = InboxStatus.Received,
                Payload = _fixture.Create<string>(),
                MessageType = _fixture.Create<string>()
            },
            new InboxMessageEntity
            {
                MessageId = receivedId,
                CorrelationId = processingId,
                Status = InboxStatus.InProgress,
                Payload = _fixture.Create<string>(),
                MessageType = _fixture.Create<string>()
            }
        );
        await _db.SaveChangesAsync();

        // Act
        await _worker.ProcessPendingMessagesAsync(CancellationToken.None);

        // Assert
        // The orchestrator should only be called for Received
        _orchestratorMock.Verify(x => x.ExecuteAsync(receivedId, It.IsAny<CancellationToken>()), Times.Once);
        _orchestratorMock.Verify(x => x.ExecuteAsync(processingId, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ProcessPendingMessagesAsync_ShouldSetStatusToCompleted_OnSuccess()
    {
        // Arrange
        Guid correlationId = _fixture.Create<Guid>();
        _db.InboxMessages.Add(new InboxMessageEntity
        {
            MessageId = correlationId,
            CorrelationId = correlationId,
            Status = InboxStatus.Received,
            Payload = _fixture.Create<string>(),
            MessageType = _fixture.Create<string>(),
        });
        await _db.SaveChangesAsync();

        _orchestratorMock.Setup(x => x.ExecuteAsync(correlationId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _worker.ProcessPendingMessagesAsync(CancellationToken.None);

        // Assert
        InboxMessageEntity? message = await _db.InboxMessages.FirstOrDefaultAsync(x => x.CorrelationId == correlationId);
        message.Should().BeNull();
    }

    [Fact]
    public async Task ProcessPendingMessagesAsync_ShouldSetStatusToFailed_WhenOrchestratorThrows()
    {
        // Arrange
        Guid correlationId = _fixture.Create<Guid>();
        _db.InboxMessages.Add(new InboxMessageEntity
        {
            MessageId = correlationId,
            CorrelationId = correlationId,
            Status = InboxStatus.Received,
            Payload = _fixture.Create<string>(),
            MessageType = _fixture.Create<string>(),
            UserUid = _fixture.Create<Guid>(),
        });
        await _db.SaveChangesAsync();

        _orchestratorMock.Setup(x => x.ExecuteAsync(correlationId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Saga Critical Failure"));

        // Act
        await _worker.ProcessPendingMessagesAsync(CancellationToken.None);

        // Assert
        InboxMessageEntity message = await _db.InboxMessages.FirstAsync(x => x.CorrelationId == correlationId);
        message.Status.Should().Be(InboxStatus.Failed);
        _loggerMock.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to process message")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    public void Dispose() => _db.Dispose();
}
