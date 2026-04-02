using AutoFixture;
using Confluent.Kafka;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.Etl.Core.Services;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Core.Tests.Services.Kafka;

public sealed class BaseInboxConsumerTests
{
    private readonly Mock<ILogger> _loggerMock = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<IServiceScope> _scopeMock = new();
    private readonly Mock<IServiceProvider> _serviceProviderMock = new();
    private readonly Mock<IInboxService> _inboxServiceMock = new();
    private readonly Fixture _autoFixture = new();

    /// <summary>
    /// Test implementation of an abstract class.
    /// </summary>
    private class TestBaseConsumer(ILogger logger, IServiceScopeFactory sf)
        : BaseInboxConsumer<string, string>(logger, sf, "topic", "localhost", "group")
    {
#nullable disable
        public InboxMessageEntity ExpectedEntity { get; set; }
#nullable restore
        public override InboxMessageEntity CreateInboxEntity(string value) => ExpectedEntity;
    }

    public BaseInboxConsumerTests()
    {
        // Setting up a DI chain: ScopeFactory -> Scope -> ServiceProvider -> IInboxService
        _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(_scopeMock.Object);
        _scopeMock.Setup(x => x.ServiceProvider).Returns(_serviceProviderMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(IInboxService)))
            .Returns(_inboxServiceMock.Object);
    }

    [Fact]
    public async Task SaveToInboxAsync_ShouldResolveServiceAndStoreMessage()
    {
        // Arrange
        TestBaseConsumer consumer = new(_loggerMock.Object, _scopeFactoryMock.Object);
        InboxMessageEntity expectedEntity = _autoFixture.Create<InboxMessageEntity>();
        consumer.ExpectedEntity = expectedEntity;

        ConsumeResult<string, string> consumeResult = new()
        {
            Message = new Message<string, string> { Value = _autoFixture.Create<string>() }
        };

        // Act
        await consumer.SaveToInboxAsync(consumeResult, CancellationToken.None);

        // Assert
        // Check that the service was called with the exact entity returned by CreateInboxEntity
        _inboxServiceMock.Verify(x => x.StoreMessageAsync(
            It.Is<InboxMessageEntity>(e => e == expectedEntity),
            It.IsAny<CancellationToken>()),
        Times.Once);

        // Check that the Scope was correctly created and disposed of (thanks to using)
        _scopeFactoryMock.Verify(x => x.CreateScope(), Times.Once);
        _scopeMock.Verify(x => x.Dispose(), Times.Once);
    }

    [Fact]
    public async Task SaveToInboxAsync_WhenServiceMissing_ShouldThrowException()
    {
        // Arrange
#nullable disable
        _serviceProviderMock.Setup(x => x.GetService(typeof(IInboxService))).Returns(null);
#nullable restore
        TestBaseConsumer consumer = new(_loggerMock.Object, _scopeFactoryMock.Object);
        ConsumeResult<string, string> consumeResult = new()
        {
            Message = new Message<string, string> { Value = _autoFixture.Create<string>() }
        };

        // Act
        Func<Task> act = () => consumer.SaveToInboxAsync(consumeResult, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
