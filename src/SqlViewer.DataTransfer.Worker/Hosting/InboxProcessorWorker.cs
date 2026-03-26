using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Sagas;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.DataTransfer.Worker.Hosting;

public sealed class InboxProcessorWorker(
    ILogger<InboxProcessorWorker> logger,
    IServiceScopeFactory scopeFactory,
    IDataTransferSagaOrchestrator orchestrator) : BackgroundService
{
    private const int PollingDelayMs = 5000;
    private const int MessageBatchSize = 10;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("InboxProcessorWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing Inbox messages.");
            }

            await Task.Delay(PollingDelayMs, stoppingToken);
        }
    }

    public async Task ProcessPendingMessagesAsync(CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        // 1. Take messages in the Received (or Pending) status that haven't yet been processed.
        // Use OrderBy to maintain the order of receipt.
        List<InboxMessageEntity> messages = await db.InboxMessages
            .Where(m => m.Status == InboxStatus.Received)
            .OrderBy(m => m.ReceivedAt)
            .Take(MessageBatchSize)
            .ToListAsync(ct);

        if (messages.Count == 0) return;

        logger.LogInformation("Found {Count} new messages in Inbox.", messages.Count);

        foreach (InboxMessageEntity message in messages)
        {
            try
            {
                // 2. Mark the message as being processed so that other workers don't take it.
                message.Status = InboxStatus.InProgress;
                await db.SaveChangesAsync(ct);

                // 3. Launching Saga via Orchestrator
                // Important: Saga automatically updates statuses and writes to Outlook.
                await orchestrator.ExecuteAsync(message.CorrelationId, ct);

                // 4. Mark as successfully completed
                message.Status = InboxStatus.Completed;
                message.ProcessedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process message {Id} with CorrelationId {CorrId}", message.Id, message.CorrelationId);

                message.Status = InboxStatus.Failed;
                message.RetryCount++;
            }

            await db.SaveChangesAsync(ct);
        }
    }
}
