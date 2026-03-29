using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Hosting;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Hosting;

public sealed class OutboxPublisherTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IKafkaProducer> _producerMock;
    private readonly Mock<ILogger<OutboxPublisherWorker>> _loggerMock;
    private readonly DbContextOptions<DataTransferDbContext> _options;

    public OutboxPublisherTests()
    {
        _fixture = new Fixture();
        _producerMock = new Mock<IKafkaProducer>();
        _loggerMock = new Mock<ILogger<OutboxPublisherWorker>>();

        _options = new DbContextOptionsBuilder<DataTransferDbContext>()
            .UseInMemoryDatabase(databaseName: _fixture.Create<string>())
            .Options;
    }

    [Fact]
    public async Task PublishOutboxMessagesAsync_ShouldDeleteMessages_WhenPublishedSuccessfully()
    {
        // Arrange
        using DataTransferDbContext context = new(_options);
        List<OutboxMessageEntity> messages = _fixture.Build<OutboxMessageEntity>()
            .With(m => m.ProcessedAt, (DateTime?)null)
            .With(m => m.RetryCount, 0)
            .CreateMany(3).ToList();

        context.OutboxMessages.AddRange(messages);
        await context.SaveChangesAsync();

        Mock<IServiceScopeFactory> scopeFactoryMock = CreateScopeFactoryMock(context);
        OutboxPublisherWorker sut = new(_loggerMock.Object, scopeFactoryMock.Object, _producerMock.Object);

        // Act
        await sut.PublishOutboxMessagesAsync(CancellationToken.None);

        // Assert
        using DataTransferDbContext assertContext = new(_options);
        assertContext.OutboxMessages.Should().BeEmpty(because: "Successful messages should be removed");
        _producerMock.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));
    }

    [Fact]
    public async Task PublishOutboxMessagesAsync_ShouldIncrementRetryCount_WhenPublishFails()
    {
        // Arrange
        using DataTransferDbContext context = new(_options);
        OutboxMessageEntity message = _fixture.Build<OutboxMessageEntity>()
            .With(m => m.ProcessedAt, (DateTime?)null)
            .With(m => m.RetryCount, 0)
            .Create();

        context.OutboxMessages.Add(message);
        await context.SaveChangesAsync();

        _producerMock
            .Setup(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Kafka error"));

        Mock<IServiceScopeFactory> scopeFactoryMock = CreateScopeFactoryMock(context);
        OutboxPublisherWorker sut = new(_loggerMock.Object, scopeFactoryMock.Object, _producerMock.Object);

        // Act
        await sut.PublishOutboxMessagesAsync(CancellationToken.None);

        // Assert
        using DataTransferDbContext assertContext = new(_options);
        OutboxMessageEntity updatedMessage = assertContext.OutboxMessages.Single();
        updatedMessage.RetryCount.Should().Be(1);
        updatedMessage.Error.Should().Be("Kafka error");
    }

    private static Mock<IServiceScopeFactory> CreateScopeFactoryMock(DataTransferDbContext context)
    {
        Mock<IServiceScope> scopeMock = new();
        Mock<IServiceProvider> serviceProviderMock = new();

        serviceProviderMock
            .Setup(x => x.GetService(typeof(DataTransferDbContext)))
            .Returns(context);

        scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);

        Mock<IServiceScopeFactory> scopeFactoryMock = new();
        scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);

        return scopeFactoryMock;
    }
}
