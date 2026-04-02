using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.Etl.Worker.Hosting;
using SqlViewer.Shared.Messages.Etl.Events;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.Etl.Worker.Tests.Unit.Hosting;

public sealed class KafkaConsumerWorkerTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<ILogger<KafkaConsumerWorker>> _loggerMock = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<IConfiguration> _configMock = new();

    [Fact]
    public void CreateInboxEntity_WithValidJson_ShouldReturnCorrectEntity()
    {
        // Arrange
        KafkaConsumerWorker worker = new(
            _loggerMock.Object,
            _scopeFactoryMock.Object,
            _configMock.Object);

        DataTransferStatusUpdated statusUpdate = _fixture.Create<DataTransferStatusUpdated>();
        string jsonPayload = JsonSerializer.Serialize(statusUpdate);

        // Act
        InboxMessageEntity result = worker.CreateInboxEntity(jsonPayload);

        // Assert
        result.Should().BeEquivalentTo(new InboxMessageEntity
        {
            MessageId = statusUpdate.MessageId,
            CorrelationId = statusUpdate.CorrelationId,
            MessageType = nameof(DataTransferStatusUpdated),
            Payload = jsonPayload,
            Status = InboxStatus.Received
        }, options => options.Excluding(x => x.ReceivedAt));
    }

    [Fact]
    public void CreateInboxEntity_WithInvalidJson_ShouldThrowException()
    {
        // Arrange
        KafkaConsumerWorker worker = new(
            _loggerMock.Object,
            _scopeFactoryMock.Object,
            _configMock.Object);

        string invalidJson = "{ invalid }";

        // Act
        Action act = () => worker.CreateInboxEntity(invalidJson);

        // Assert
        act.Should().Throw<Exception>()
            .And.Message.Should().NotBeNullOrEmpty();
    }
}