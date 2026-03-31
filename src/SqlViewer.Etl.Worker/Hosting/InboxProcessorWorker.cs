using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Worker.Services;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.Etl.Worker.Hosting;

public sealed class InboxProcessorWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<InboxProcessorWorker> logger) : BackgroundService
{
    private const int BatchSize = 20;
    private const int DelayMs = 1000;
    private const int ErrorDelayMs = 5000;
    private const int MaxRetryCount = 5;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessages(ct);
                await Task.Delay(DelayMs, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fatal error in InboxProcessorWorker loop");
                await Task.Delay(ErrorDelayMs, ct);
            }
        }
    }

    public async Task ProcessPendingMessages(CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        EtlDbContext db = scope.ServiceProvider.GetRequiredService<EtlDbContext>();

        // Get a batch of unprocessed messages
        List<InboxMessageEntity> messages = await db.InboxMessages
            .Where(m => m.Status == InboxStatus.Received && m.RetryCount < 5)
            .OrderBy(m => m.ReceivedAt)
            .Take(BatchSize)
            .ToListAsync(ct);

        if (messages.Count == 0) return;

        // Group messages by CorrelationId to ensure sequential processing for the same job
        IEnumerable<IGrouping<Guid, InboxMessageEntity>> groupedMessages = messages
            .GroupBy(m => m.CorrelationId);

        List<Task> tasks = groupedMessages.Select(async group =>
        {
            // Process all messages for one CorrelationId sequentially
            foreach (InboxMessageEntity message in group)
            {
                await ProcessSingleMessageAsync(message, ct);
            }
        }).ToList();

        // Run processing for different CorrelationIds in parallel
        await Task.WhenAll(tasks);
    }

    public async Task ProcessSingleMessageAsync(InboxMessageEntity message, CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        EtlDbContext db = scope.ServiceProvider.GetRequiredService<EtlDbContext>();

        ITransferStatusService? handler = scope.ServiceProvider.GetService<ITransferStatusService>();

        if (handler == null)
        {
            logger.LogWarning("No handler found for message type: {Type}", message.MessageType);
            return;
        }

        try
        {
            // Ensure the entity is tracked by the current context for deletion
            db.Entry(message).State = EntityState.Unchanged;

            await handler.ProcessAsync(message, ct);

            db.InboxMessages.Remove(message);

            await db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process inbox message {Id}. Attempting to update retry count.", message.Id);

            // Update retry status in a separate operation to ensure it persists even after business logic failure
            await UpdateMessageFailureStatus(message.Id, ex.Message, ct);
        }
    }

    public async Task UpdateMessageFailureStatus(long id, string errorMessage, CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        EtlDbContext db = scope.ServiceProvider.GetRequiredService<EtlDbContext>();

        InboxMessageEntity? message = await db.InboxMessages.FindAsync([id], ct);
        if (message == null) return;

        message.RetryCount++;
        message.Error = errorMessage;
        if (message.RetryCount >= MaxRetryCount)
        {
            message.Status = InboxStatus.Failed;
        }

        await db.SaveChangesAsync(ct);
    }
}
