using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Hosting;
using SqlViewer.Shared.Messages.Etl.Commands;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Hosting;

public sealed class KafkaConsumerWorkerTests
{
    private readonly Mock<ILogger<KafkaConsumerWorker>> _loggerMock = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Fixture _autoFixture = new();

    public KafkaConsumerWorkerTests()
    {
        _configMock.Setup(c => c[It.IsAny<string>()]).Returns(_autoFixture.Create<string>());
    }

    [Fact]
    public void CreateInboxEntity_WithValidJson_ShouldMapFieldsCorrectly()
    {
        // Arrange
        Guid userUid = _autoFixture.Create<Guid>();

        KafkaConsumerWorker consumer = new(_loggerMock.Object, _scopeFactoryMock.Object, _configMock.Object);
        StartDataTransferCommand command = _autoFixture.Build<StartDataTransferCommand>()
            .With(c => c.UserUid, userUid.ToString())
            .Create();
        string json = JsonSerializer.Serialize(command);

        // Act
        InboxMessageEntity result = consumer.CreateInboxEntity(json);

        // Assert
        result.Should().BeEquivalentTo(new
        {
            command.CorrelationId,
            UserUid = userUid,
            MessageType = nameof(StartDataTransferCommand),
            Payload = json
        }, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void CreateInboxEntity_WithInvalidUserUid_ShouldThrowInvalidOperationException()
    {
        // Arrange
        KafkaConsumerWorker consumer = new(_loggerMock.Object, _scopeFactoryMock.Object, _configMock.Object);
        var invalidCommand = new { CorrelationId = _autoFixture.Create<Guid>(), UserUid = "not-a-guid" };
        string json = JsonSerializer.Serialize(invalidCommand);

        // Act
        Action act = () => consumer.CreateInboxEntity(json);

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage($"Unable to parse UserUid for {nameof(StartDataTransferCommand)}");
    }
}
