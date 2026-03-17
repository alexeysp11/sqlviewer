using Grpc.Core;
using MassTransit;
using SqlViewer.Common.Messages.Etl.Commands;
using Google.Protobuf.WellKnownTypes;

namespace SqlViewer.Etl.Services;

public class EtlGrpcService(IBus bus) : EtlTransferService.EtlTransferServiceBase
{
    private readonly IBus _bus = bus;

    public override async Task<StartTransferResponse> StartTransfer(
        StartTransferRequest request,
        ServerCallContext context)
    {
        Guid correlationId = Guid.NewGuid();

        // Sending a command to Kafka via MassTransit
        await _bus.Publish(new StartDataTransferCommand(
            correlationId,
            request.SourceConnectionString,
            request.TargetConnectionString,
            request.TableName
        ));

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
