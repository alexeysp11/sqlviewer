using Grpc.Core;
using MassTransit;
using SqlViewer.Common.Messages.Etl.Commands;
using Google.Protobuf.WellKnownTypes;

namespace SqlViewer.Etl.Services.Grpc;

public sealed class EtlGrpcService(
    ITopicProducer<StartDataTransferCommand> startTransferProducer)
    : EtlTransferService.EtlTransferServiceBase
{
    public override async Task<StartTransferResponse> StartTransfer(
        StartTransferRequest request,
        ServerCallContext context)
    {
        Guid correlationId = Guid.NewGuid();

        // Sending a command to Kafka via MassTransit
        // TODO: handle the situation when Kafka is not responding so as not to freeze the request from the client application.
        await startTransferProducer.Produce(new StartDataTransferCommand(
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
