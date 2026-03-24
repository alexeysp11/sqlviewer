using AutoFixture;
using Confluent.Kafka;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Etl.Worker.BackgroundWorkers;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Worker.Tests.Integration.BackgroundWorkers;

public sealed class OutboxPublisherWorkerTests
{
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock;
    private readonly Mock<IKafkaProducer> _producerMock;
    private readonly DbContextOptions<EtlDbContext> _dbOptions;

    public OutboxPublisherWorkerTests()
    {
        _scopeFactoryMock = new Mock<IServiceScopeFactory>();
        _producerMock = new Mock<IKafkaProducer>();

        _dbOptions = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessMessage()
    {
        // Arrange
        using EtlDbContext db = new(_dbOptions);
        db.OutboxMessages.Add(new()
        {
            Id = 1,
            CorrelationId = _autoFixture.Create<Guid>(),
            MessageType = _autoFixture.Create<string>(),
            Topic = "test",
            Payload = "{}",
            CreatedAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
        SetupScope(db);

        // This object will allow us to signal: "The worker has finished work!"
        TaskCompletionSource<bool> tcs = new();

        _producerMock
            .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new DeliveryResult<string, string>())
            .Callback(() => tcs.SetResult(true));

        OutboxPublisherWorker worker = new(_scopeFactoryMock.Object, _producerMock.Object);
        using CancellationTokenSource cts = new();

        // Act
        Task runTask = worker.StartAsync(cts.Token);

        // Wait for either a signal from the mock or a timeout (so that the test doesn't hang forever)
        Task completedTask = await Task.WhenAny(tcs.Task, Task.Delay(2000));

        // Stop the worker.
        cts.Cancel();
        await runTask;

        // Assert
        completedTask.Should().Be(tcs.Task, "The worker should have called ProduceAsync");

        using EtlDbContext dbVerify = new(_dbOptions);
        dbVerify.OutboxMessages.Should().BeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldIncrementRetryCount_WhenKafkaFails()
    {
        // Arrange
        using EtlDbContext db = new(_dbOptions);
        OutboxMessageEntity msg = new()
        {
            Id = 2,
            CorrelationId = _autoFixture.Create<Guid>(),
            MessageType = _autoFixture.Create<string>(),
            Topic = "test",
            Payload = "bad",
            RetryCount = 0
        };
        db.OutboxMessages.Add(msg);
        await db.SaveChangesAsync();

        SetupScope(db);

        _producerMock
            .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Kafka connection error"));

        OutboxPublisherWorker worker = new(_scopeFactoryMock.Object, _producerMock.Object);
        CancellationTokenSource cts = new();

        // Act
        Task runTask = worker.StartAsync(cts.Token);
        await Task.Delay(OutboxPublisherWorker.DelayEmptyMessagesMs);
        cts.Cancel();
        await runTask;

        // Assert
        using EtlDbContext dbVerify = new(_dbOptions);
        OutboxMessageEntity updatedMsg = dbVerify.OutboxMessages.Single();
        updatedMsg.RetryCount.Should().BeGreaterThanOrEqualTo(1);
        updatedMsg.Error.Should().Contain("Kafka connection error");
    }

    private void SetupScope(EtlDbContext db)
    {
        Mock<IServiceScope> scopeMock = new();
        Mock<IServiceProvider> serviceProviderMock = new();

        serviceProviderMock
            .Setup(x => x.GetService(typeof(EtlDbContext)))
            .Returns(db);

        scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
        _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
    }
}
