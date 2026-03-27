using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Sagas;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.DataTransfer.Worker.Hosting;

public sealed class InboxProcessorWorker(
    ILogger<InboxProcessorWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
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
        using IServiceScope rootScope = scopeFactory.CreateScope();
        DataTransferDbContext db = rootScope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        List<InboxMessageEntity> messages = await db.InboxMessages
            .Where(m => m.Status == InboxStatus.Received)
            .OrderBy(m => m.ReceivedAt)
            .Take(MessageBatchSize)
            .ToListAsync(ct);

        if (messages.Count == 0) return;

        logger.LogInformation("Found {Count} new messages in Inbox.", messages.Count);

        IEnumerable<Task> tasks = messages.Select(async message =>
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            DataTransferDbContext scopedDb = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();
            IDataTransferSagaOrchestrator scopedOrchestrator = scope.ServiceProvider.GetRequiredService<IDataTransferSagaOrchestrator>();

            try
            {
                scopedDb.Attach(message);
                message.Status = InboxStatus.InProgress;
                await scopedDb.SaveChangesAsync(ct);

                await scopedOrchestrator.ExecuteAsync(message.CorrelationId, ct);

                message.Status = InboxStatus.Completed;
                message.ProcessedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process message {Id} with CorrelationId {CorrId}", message.Id, message.CorrelationId);

                message.Status = InboxStatus.Failed;
                message.RetryCount++;
            }

            await scopedDb.SaveChangesAsync(ct);
        });

        await Task.WhenAll(tasks);
    }
}
