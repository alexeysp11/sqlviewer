using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Messages.Common.Entities;
using SqlViewer.Etl.Data.DbContexts;

namespace SqlViewer.Etl.BackgroundServices;

/// <summary>
/// Пишет в Inbox.
/// Его задача — только сохранить сырой JSON в БД.
/// </summary>
/// <param name="scopeFactory"></param>
/// <param name="logger"></param>
public class KafkaConsumerService(IServiceScopeFactory scopeFactory, ILogger<KafkaConsumerService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Здесь стандартный цикл Consumer.Consume(...) 
        // 1. Получили сообщение.
        // 2. CorrelationId достаем из заголовков или JSON.
        Guid correlationId = Guid.NewGuid();
        string json = string.Empty;

        using IServiceScope scope = scopeFactory.CreateScope();
        EtlDbContext db = scope.ServiceProvider.GetRequiredService<EtlDbContext>();

        // Проверяем на дубли сразу при входе (Idempotent Consumer)
        if (!await db.InboxMessages.AnyAsync(m => m.CorrelationId == correlationId, cancellationToken))
        {
            db.InboxMessages.Add(new InboxMessageEntity
            {
                CorrelationId = correlationId,
                Payload = json,
                MessageType = "DataTransferStarted" // Определяем по топику или полю в JSON
            });
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
