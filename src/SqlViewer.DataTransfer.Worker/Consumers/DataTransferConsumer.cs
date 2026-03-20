namespace SqlViewer.DataTransfer.Worker.Consumers;

public class DataTransferConsumer()
{
    //public async Task Consume(ConsumeContext<StartDataTransferCommand> context)
    //{
    //    StartDataTransferCommand msg = context.Message;

    //    // 1. Publish data transfer start event.
    //    await _publishEndpoint.Publish(new DataTransferStarted(
    //        msg.CorrelationId,
    //        msg.SourceConnectionString,
    //        msg.TargetConnectionString,
    //        1000,
    //        DateTime.UtcNow));

    //    try
    //    {
    //        for (int i = 1; i <= 10; i++)
    //        {
    //            await Task.Delay(500); // Simulation of work

    //            // 2. Send progress (WPF will subscribe to this topic via API Gateway)
    //            await _publishEndpoint.Publish(new DataTransferProgressUpdated(
    //                msg.CorrelationId,
    //                i * 10,
    //                $"Processing chunk {i}",
    //                i * 100,
    //                DateTime.UtcNow));
    //        }

    //        // 3. Successful completion
    //        await _publishEndpoint.Publish(new DataTransferCompleted(msg.CorrelationId, 1000, DateTime.UtcNow));
    //    }
    //    catch (Exception ex)
    //    {
    //        await _publishEndpoint.Publish(new DataTransferFailed(
    //            msg.CorrelationId,
    //            ex.Message,
    //            ex.GetType().Name,
    //            ex.StackTrace,
    //            DateTime.UtcNow));
    //    }
    //}
}

