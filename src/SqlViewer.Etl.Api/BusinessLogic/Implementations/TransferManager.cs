using SqlViewer.Etl.Api.BusinessLogic.Abstractions;
using SqlViewer.Etl.Api.Repositories.Abstractions;
using SqlViewer.Shared.Constants;

namespace SqlViewer.Etl.Api.BusinessLogic.Implementations;

public sealed class TransferManager(IOutboxRepository outboxRepository, IConfiguration configuration) : ITransferManager
{
    public async Task<Guid> InitiateTransferAsync(Guid correlationId, Guid userUid, string requestJson)
    {
        string? topic = configuration[ConfigurationKeys.Services.Kafka.Topics.DataTransferCommand];
        if (string.IsNullOrEmpty(topic))
            throw new InvalidOperationException("Kafka topic is not configured.");

        await outboxRepository.AddTransferCommandAsync(userUid, correlationId, topic, requestJson);

        return correlationId;
    }
}
