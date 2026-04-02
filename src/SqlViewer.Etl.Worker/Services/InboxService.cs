using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Services;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Worker.Services;

public sealed class InboxService(EtlDbContext db, ILogger<InboxService> logger) : IInboxService
{
    public async Task StoreMessageAsync(InboxMessageEntity message, CancellationToken ct)
    {
        bool exists = await db.InboxMessages
            .AnyAsync(m => m.MessageId == message.MessageId, ct);

        if (exists)
        {
            logger.LogWarning("Message {Id} already exists in Inbox. Skipping.", message.MessageId);
            return;
        }

        db.InboxMessages.Add(message);
        await db.SaveChangesAsync(ct);
    }
}
