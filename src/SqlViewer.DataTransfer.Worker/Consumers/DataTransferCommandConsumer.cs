using System.Text.Json;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Messages.Etl.Commands;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.DataTransfer.Worker.Consumers;

public sealed class DataTransferCommandConsumer(
    ILogger<DataTransferCommandConsumer> logger,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration) : BaseInboxConsumer<string, string>(
        logger,
        scopeFactory,
        topic: configuration[ConfigurationKeys.Services.Kafka.Topics.DataTransferCommand]!,
        bootstrapServers: configuration[ConfigurationKeys.Services.Kafka.Url]!,
        groupId: configuration[ConfigurationKeys.Services.Kafka.Groups.DataTransferWorkerGroup]!)
{
    private const string CommandName = nameof(StartDataTransferCommand);

    /// <summary>
    /// Create the inbox entity.
    /// </summary>
    protected override InboxMessageEntity CreateInboxEntity(string jsonCommand)
    {
        StartDataTransferCommand command = JsonSerializer.Deserialize<StartDataTransferCommand>(jsonCommand)
            ?? throw new InvalidOperationException($"Failed to deserialize {nameof(StartDataTransferCommand)}");

        if (!Guid.TryParse(command.UserUid, out Guid userUid))
            throw new InvalidOperationException($"Unable to get {CommandName}");

        return new InboxMessageEntity
        {
            CorrelationId = command.CorrelationId,
            UserUid = userUid,
            MessageType = CommandName,
            Payload = jsonCommand,
            ReceivedAt = DateTime.UtcNow,
            Status = InboxStatus.Received
        };
    }
}
