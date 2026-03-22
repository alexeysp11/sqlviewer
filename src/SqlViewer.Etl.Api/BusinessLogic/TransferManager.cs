using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Shared.Constants;

namespace SqlViewer.Etl.Api.BusinessLogic;

public sealed class TransferManager(IOutboxRepository outboxRepository, IConfiguration configuration) : ITransferManager
{
    public async Task<Guid> InitiateTransferAsync(Guid userUid, string requestJson)
    {
        Guid correlationId = Guid.NewGuid();

        string? topic = configuration[ConfigurationKeys.Services.Kafka.Topics.DataTransferCommand];
        if (string.IsNullOrEmpty(topic))
            throw new InvalidOperationException("Kafka topic is not configured.");

        await outboxRepository.AddTransferCommandAsync(userUid, correlationId, topic, requestJson);

        return correlationId;
    }
}
