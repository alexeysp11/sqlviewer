using AutoFixture;
using Confluent.Kafka;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Etl.Worker.BackgroundWorkers;
using SqlViewer.Shared.Messages.Storage.Entities;
using VelocipedeUtils.Shared.DbOperations.Enums;
using static SqlViewer.Shared.Constants.ConfigurationKeys;

namespace SqlViewer.Etl.Worker.Tests.Integration.BackgroundWorkers;

public sealed class OutboxPublisherWorkerTests
{
    private readonly Fixture _autoFixture = new();
    private readonly Mock<IKafkaProducer> _producerMock;
    private readonly DbContextOptions<EtlDbContext> _dbOptions;

    public OutboxPublisherWorkerTests()
    {
        _producerMock = new Mock<IKafkaProducer>();

        _dbOptions = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(databaseName: _autoFixture.Create<Guid>().ToString())
            .Options;
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessMessage_AndCleanOutbox()
    {
        // Arrange
        Guid correlationId = _autoFixture.Create<Guid>();
        string topic = _autoFixture.Create<string>();
        string payload = _autoFixture.Create<string>();

        // Use different contexts for preparation and testing.
        using (EtlDbContext dbSetup = new(_dbOptions))
        {
            dbSetup.OutboxMessages.Add(new()
            {
                Id = 1,
                CorrelationId = correlationId,
                MessageType = _autoFixture.Create<string>(),
                Topic = topic,
                Payload = payload,
                CreatedAt = DateTime.UtcNow
            });
            dbSetup.TransferJobs.Add(new()
            {
                CorrelationId = correlationId,
                UserUid = _autoFixture.Create<Guid>(),
                SourceConnectionString = _autoFixture.Create<string>(),
                TargetConnectionString = _autoFixture.Create<string>(),
                SourceDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TargetDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TableName = _autoFixture.Create<string>(),
                CurrentStatus = TransferStatus.None,
            });
            await dbSetup.SaveChangesAsync();
        }
        IServiceScopeFactory scopeFactory = SetupScope(new(_dbOptions));

        TaskCompletionSource<bool> tcs = new();
        _producerMock
            .Setup(p => p.ProduceAsync(topic, correlationId.ToString(), payload))
            .ReturnsAsync(new DeliveryResult<string, string>())
            .Callback(() => tcs.SetResult(true));

        // Configurations.
        Dictionary<string, string?> configurationDictionary = new()
        {
            {Services.Observability.ServiceName, _autoFixture.Create<string>()}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationDictionary)
            .Build();

        OutboxPublisherWorker worker = new(
            Mock.Of<ILogger<OutboxPublisherWorker>>(),
            configuration,
            scopeFactory,
            _producerMock.Object);

        // Act
        await worker.ProcessBatchAsync(CancellationToken.None);

        // Assert
        // Check the status in the database using a new context
        using (EtlDbContext dbAssert = new(_dbOptions))
        {
            dbAssert.OutboxMessages.Should().BeEmpty();

            TransferJobEntity? job = await dbAssert.TransferJobs
                .Include(x => x.Logs)
                .FirstOrDefaultAsync(x => x.CorrelationId == correlationId);

            job.Should().NotBeNull();
            job!.CurrentStatus.Should().Be(TransferStatus.Queued);
            job.Logs.Should().ContainSingle(l => l.Status == TransferStatus.Queued);
        }

        _producerMock.VerifyAll();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldIncrementRetryCount_WhenKafkaFails()
    {
        // Arrange
        // 1. DbContext.
        using (EtlDbContext db = new(_dbOptions))
        {
            OutboxMessageEntity msg = new()
            {
                Id = 2,
                CorrelationId = _autoFixture.Create<Guid>(),
                MessageType = _autoFixture.Create<string>(),
                Topic = _autoFixture.Create<string>(),
                Payload = _autoFixture.Create<string>(),
                RetryCount = 0
            };
            db.OutboxMessages.Add(msg);
            await db.SaveChangesAsync();
        }
        IServiceScopeFactory scopeFactory = SetupScope(new(_dbOptions));

        // 2. Mocks.
        // Kafka producer.
        _producerMock
            .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Kafka connection error"));

        // Configurations.
        Dictionary<string, string?> configurationDictionary = new()
        {
            {Services.Observability.ServiceName, _autoFixture.Create<string>()}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationDictionary)
            .Build();

        // Act
        OutboxPublisherWorker worker = new(
            Mock.Of<ILogger<OutboxPublisherWorker>>(),
            configuration,
            scopeFactory,
            _producerMock.Object);
        await worker.ProcessBatchAsync(CancellationToken.None);

        // Assert
        using (EtlDbContext dbAssert = new(_dbOptions))
        {
            OutboxMessageEntity updatedMsg = dbAssert.OutboxMessages.Single();
            updatedMsg.RetryCount.Should().Be(1);
            updatedMsg.Error.Should().Contain("Kafka connection error");
        }
    }

    private static IServiceScopeFactory SetupScope(EtlDbContext db)
    {
        Mock<IServiceScope> scopeMock = new();
        Mock<IServiceProvider> serviceProviderMock = new();

        serviceProviderMock
            .Setup(x => x.GetService(typeof(EtlDbContext)))
            .Returns(db);

        scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);

        Mock<IServiceScopeFactory> scopeFactoryMock = new();
        scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);

        return scopeFactoryMock.Object;
    }
}
