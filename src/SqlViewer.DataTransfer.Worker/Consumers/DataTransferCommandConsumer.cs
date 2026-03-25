using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Messages.Etl.Commands;

namespace SqlViewer.DataTransfer.Worker.Consumers;

public sealed class DataTransferCommandConsumer(
    ILogger<DataTransferCommandConsumer> logger,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration) : BaseInboxConsumer<string, StartDataTransferCommand>(
        logger,
        scopeFactory,
        topic: "data-transfer-commands",
        bootstrapServers: configuration["Services:Kafka:Url"]!,
        groupId: "data-transfer-worker-group")
{

    /// <summary>
    /// Extracts the CorrelationId directly from the strongly-typed command.
    /// </summary>
    protected override Guid ExtractCorrelationId(StartDataTransferCommand command)
    {
        return command.CorrelationId;
    }
}
