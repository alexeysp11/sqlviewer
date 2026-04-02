using System.Text.Json;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Messages.Etl.Commands;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.DataTransfer.Worker.Hosting;

/// <summary>
/// Data transfer class for Kafka consumers that implements the Inbox pattern.
/// Persists incoming messages to the database before processing.
/// </summary>
public sealed class KafkaConsumerWorker(
    ILogger<KafkaConsumerWorker> logger,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration) : BaseInboxConsumer<string, string>(
        logger,
        scopeFactory,
        topic: configuration[ConfigurationKeys.Services.Kafka.Topics.DataTransferCommand]!,
        bootstrapServers: configuration[ConfigurationKeys.Services.Kafka.Url]!,
        groupId: configuration[ConfigurationKeys.Services.Kafka.Groups.DataTransferWorkerGroup]!)
{
    private const string CommandName = nameof(StartDataTransferCommand);

    /// <inheritdoc/>
    public override InboxMessageEntity CreateInboxEntity(string jsonCommand)
    {
        StartDataTransferCommand command = JsonSerializer.Deserialize<StartDataTransferCommand>(jsonCommand)
            ?? throw new InvalidOperationException($"Failed to deserialize {nameof(StartDataTransferCommand)}");

        if (!Guid.TryParse(command.UserUid, out Guid userUid))
            throw new InvalidOperationException($"Unable to parse UserUid for {CommandName}");

        return new InboxMessageEntity
        {
            // For the initial command, using CorrelationId as MessageId is acceptable 
            // to ensure the entire transfer process is triggered only once.
            MessageId = command.CorrelationId,
            CorrelationId = command.CorrelationId,
            UserUid = userUid,
            MessageType = CommandName,
            Payload = jsonCommand,
            ReceivedAt = DateTime.UtcNow,
            Status = InboxStatus.Received
        };
    }
}
