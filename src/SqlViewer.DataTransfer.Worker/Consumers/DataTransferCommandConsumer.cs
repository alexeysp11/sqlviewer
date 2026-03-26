using System.Text.Json;
using SqlViewer.Etl.Core.Services.Kafka;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Messages.Etl.Commands;

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
    /// <summary>
    /// Extracts the CorrelationId directly from the strongly-typed command.
    /// </summary>
    protected override Guid ExtractCorrelationId(string jsonCommand)
    {
        StartDataTransferCommand command = JsonSerializer.Deserialize<StartDataTransferCommand>(jsonCommand)
            ?? throw new InvalidOperationException($"Unable to parse JSON to get {nameof(StartDataTransferCommand)}");
        return command.CorrelationId;
    }
}
