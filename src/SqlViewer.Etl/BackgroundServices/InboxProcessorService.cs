using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Messages.Common.Enums;
using SqlViewer.Etl.Data.DbContexts;
using SqlViewer.Etl.Sagas;

namespace SqlViewer.Etl.BackgroundServices;

/// <summary>
/// Из Inbox в Сагу.
/// </summary>
/// <param name="scopeFactory"></param>
public class InboxProcessorService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using IServiceScope scope = scopeFactory.CreateScope();
            EtlDbContext db = scope.ServiceProvider.GetRequiredService<EtlDbContext>();
            DataTransferSagaOrchestrator orchestrator = scope.ServiceProvider.GetRequiredService<DataTransferSagaOrchestrator>();

            Common.Messages.Common.Entities.InboxMessageEntity? msg = await db.InboxMessages
                .FirstOrDefaultAsync(m => m.Status == InboxStatus.Received, cancellationToken);
            if (msg != null)
            {
                // Десериализуем Payload в объект команды на основе MessageType
                //object command = Deserialize(msg.Payload, msg.MessageType);
                object command = new { Message = "Hello world" };

                await orchestrator.HandleAsync(msg.CorrelationId, command);

                msg.Status = InboxStatus.Completed;
                msg.ProcessedAt = DateTime.UtcNow;
                await db.SaveChangesAsync(cancellationToken);
            }
            await Task.Delay(100, cancellationToken);
        }
    }
}
