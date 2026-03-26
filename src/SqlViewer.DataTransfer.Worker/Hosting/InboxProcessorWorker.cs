using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Shared.Messages.Storage.Enums;

namespace SqlViewer.DataTransfer.Worker.Hosting;

/// <summary>
/// Из Inbox в Сагу.
/// </summary>
/// <param name="scopeFactory"></param>
public class InboxProcessorWorker(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

            InboxMessageEntity? msg = await db.InboxMessages
                .FirstOrDefaultAsync(m => m.Status == InboxStatus.Received, cancellationToken);
            if (msg != null)
            {
                // Десериализуем Payload в объект команды на основе MessageType
                //object command = Deserialize(msg.Payload, msg.MessageType);
                object command = new { Message = "Hello world" };

                msg.Status = InboxStatus.Completed;
                msg.ProcessedAt = DateTime.UtcNow;
                await db.SaveChangesAsync(cancellationToken);
            }
            await Task.Delay(100, cancellationToken);
        }
    }
}
