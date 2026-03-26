using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Consumers;
using SqlViewer.Shared.Messages.Etl.Commands;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Consumers;

public sealed class DataTransferCommandConsumerTests
{
    private readonly Mock<ILogger<DataTransferCommandConsumer>> _loggerMock = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Fixture _autoFixture = new();

    public DataTransferCommandConsumerTests()
    {
        _configMock.Setup(c => c[It.IsAny<string>()]).Returns(_autoFixture.Create<string>());
    }

    [Fact]
    public void CreateInboxEntity_WithValidJson_ShouldMapFieldsCorrectly()
    {
        // Arrange
        Guid userUid = _autoFixture.Create<Guid>();

        DataTransferCommandConsumer consumer = new(_loggerMock.Object, _scopeFactoryMock.Object, _configMock.Object);
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
        DataTransferCommandConsumer consumer = new(_loggerMock.Object, _scopeFactoryMock.Object, _configMock.Object);
        var invalidCommand = new { CorrelationId = _autoFixture.Create<Guid>(), UserUid = "not-a-guid" };
        string json = JsonSerializer.Serialize(invalidCommand);

        // Act
        Action act = () => consumer.CreateInboxEntity(json);

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*Unable to get StartDataTransferCommand*");
    }
}
