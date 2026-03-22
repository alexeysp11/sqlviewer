using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SqlViewer.Etl.Api.BusinessLogic;

namespace SqlViewer.Etl.Api.Services.Grpc;

public sealed class EtlGrpcService(ITransferManager transferManager) : EtlTransferService.EtlTransferServiceBase
{
    public override async Task<StartTransferResponse> StartTransfer(
        StartTransferRequest request,
        ServerCallContext context)
    {
        if (request.UserUid is null || !Guid.TryParse(request.UserUid, out Guid userUid))
            throw new InvalidOperationException("ETL transfer is available only to authorized users");

        Guid correlationId = Guid.NewGuid();

        // Convert request to JSON.
        JsonFormatter.Settings settings = new JsonFormatter.Settings(formatDefaultValues: true, typeRegistry: null)
            .WithFormatEnumsAsIntegers(false);
        JsonFormatter formatter = new(settings);
        string requestJson = formatter.Format(request);

        // Start transfer.
        await transferManager.InitiateTransferAsync(userUid, requestJson);

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
