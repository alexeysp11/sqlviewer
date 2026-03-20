using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Messages.Etl.Commands;
using Google.Protobuf;

namespace SqlViewer.Etl.Api.Services.Grpc;

public sealed class EtlGrpcService(
    EtlDbContext dbContext,
    IConfiguration configuration) : EtlTransferService.EtlTransferServiceBase
{
    public override async Task<StartTransferResponse> StartTransfer(
        StartTransferRequest request,
        ServerCallContext context)
    {
        Guid correlationId = Guid.NewGuid();

        // Convert request to JSON.
        JsonFormatter.Settings settings = new JsonFormatter.Settings(formatDefaultValues: true, typeRegistry: null)
            .WithFormatEnumsAsIntegers(false);
        JsonFormatter formatter = new(settings);
        string requestJson = formatter.Format(request);

        // Save a record in outbox.
        await dbContext.OutboxMessages.AddAsync(new()
        {
            CorrelationId = correlationId,
            Topic = configuration[ConfigurationKeys.Services.Kafka.Topics.DataTransferCommand]!,
            MessageType = nameof(StartDataTransferCommand),
            Payload = requestJson
        });

        return new StartTransferResponse
        {
            CorrelationId = correlationId.ToString(),
            CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow),
            Message = "Transfer request accepted and placed in queue."
        };
    }

    // We'll leave the stream empty for now or implement it later via ResponseStream.WriteAsync
    public override async Task SubscribeToTransferStatus(
        StatusSubscriptionRequest request,
        IServerStreamWriter<TransferStatusUpdate> responseStream,
        ServerCallContext context)
    {
        // We listen to Kafka and update the current status in Redis.
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000);
        }
    }
}
