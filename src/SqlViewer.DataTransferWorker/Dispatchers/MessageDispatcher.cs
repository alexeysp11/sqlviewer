namespace SqlViewer.DataTransferWorker.Dispatchers;

/// <summary>
/// Background service that consumes Kafka and populates the Inbox table.
/// </summary>
public class MessageDispatcher : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // 1. Consume from Kafka using Confluent.Kafka
        // 2. Deserialize Metadata to get CorrelationId and MessageType
        // 3. Start DB Transaction:
        //    a. Check if CorrelationId exists in Inbox (Ignore if yes)
        //    b. Insert into InboxMessageEntity { Status = Received }
        // 4. Commit Transaction and Commit Kafka Offset
    }
}
