using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Messages.Etl.Events;
using SqlViewer.Shared.Messages.Storage.Enums;
using System.Text.Json;
using SqlViewer.Shared.Constants;

namespace SqlViewer.Etl.Worker.Hosting;

public sealed class KafkaConsumerWorker(
    ILogger<KafkaConsumerWorker> logger,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration) : BaseInboxConsumer<string, string>(
        logger,
        scopeFactory,
        topic: configuration[ConfigurationKeys.Services.Kafka.Topics.DataTransferStatusUpdates]!,
        bootstrapServers: configuration[ConfigurationKeys.Services.Kafka.Url]!,
        groupId: configuration[ConfigurationKeys.Services.Kafka.Groups.EtlWorkerGroup]!)
{
    /// <inheritdoc/>
    public override InboxMessageEntity CreateInboxEntity(string value)
    {
        DataTransferStatusUpdated statusUpdate = JsonSerializer.Deserialize<DataTransferStatusUpdated>(value)
            ?? throw new InvalidOperationException("Failed to deserialize Kafka message payload.");

        return new InboxMessageEntity
        {
            MessageId = statusUpdate.MessageId,
            CorrelationId = statusUpdate.CorrelationId,
            MessageType = nameof(DataTransferStatusUpdated),
            Payload = value,
            Status = InboxStatus.Received,
            ReceivedAt = DateTime.UtcNow
        };
    }
}
