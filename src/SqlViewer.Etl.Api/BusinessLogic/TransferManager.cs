using SqlViewer.Shared.Constants;
using SqlViewer.Etl.Api.Repositories;

namespace SqlViewer.Etl.Api.BusinessLogic;

public class TransferManager(IOutboxRepository outboxRepository, IConfiguration configuration) : ITransferManager
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
