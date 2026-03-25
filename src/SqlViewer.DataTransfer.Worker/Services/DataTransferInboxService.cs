using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.Etl.Core.Services;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Services;

public sealed class DataTransferInboxService(DataTransferDbContext db, ILogger<DataTransferInboxService> logger) : IInboxService
{
    public async Task StoreMessageAsync(InboxMessageEntity message, CancellationToken ct)
    {
        // Basic Idempotency check:
        bool exists = await db.InboxMessages.AnyAsync(m => m.CorrelationId == message.CorrelationId, ct);

        if (exists)
        {
            logger.LogWarning("Message with CorrelationId {Id} already exists in Inbox. Skipping.", message.CorrelationId);
            return;
        }

        db.InboxMessages.Add(message);
        await db.SaveChangesAsync(ct);
    }
}
