using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using SqlViewer.Etl.Api.BusinessLogic.Implementations;
using SqlViewer.Etl.Api.Repositories.Abstractions;
using SqlViewer.Shared.Constants;

namespace SqlViewer.Etl.Api.Tests.Unit.BusinessLogic;

public sealed class TransferManagerTests
{
    private readonly Mock<IOutboxRepository> _repositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly TransferManager _manager;

    public TransferManagerTests()
    {
        _repositoryMock = new Mock<IOutboxRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _manager = new TransferManager(_repositoryMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task InitiateTransferAsync_ValidRequest_CallsRepositoryWithCorrectData()
    {
        // Arrange.
        Guid userUid = Guid.NewGuid();
        Guid correlationId = Guid.NewGuid();
        string expectedTopic = "test-transfer-topic";
        string requestJson = "{\"Id\": 123, \"Command\": \"Start\"}";

        // Configure the config so that it returns our topic by key.
        _configurationMock
            .Setup(c => c[ConfigurationKeys.Services.Kafka.Topics.DataTransferCommand])
            .Returns(expectedTopic);

        // Act.
        Guid resultCorrelationId = await _manager.InitiateTransferAsync(correlationId, userUid, requestJson);

        // Assert.
        // 1. Check that CorrelationId is not empty
        Assert.NotEqual(Guid.Empty, correlationId);

        // 2. Check that the repository was called exactly once with the expected parameters
        _repositoryMock.Verify(r => r.AddTransferCommandAsync(
            It.Is<Guid>(g => g == userUid),
            It.Is<Guid>(g => g == correlationId),
            expectedTopic,
            requestJson),
        Times.Once);
        resultCorrelationId.Should().Be(correlationId);
    }

    [Fact]
    public async Task InitiateTransferAsync_MissingConfig_ThrowsException()
    {
        // Arrange
        // Simulating the absence of a key in the config
        Guid correlationId = Guid.NewGuid();
        Guid userUid = Guid.NewGuid();
        _configurationMock
            .Setup(c => c[ConfigurationKeys.Services.Kafka.Topics.DataTransferCommand])
            .Returns((string)null!);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _manager.InitiateTransferAsync(correlationId, userUid, "{}"));
    }
}
