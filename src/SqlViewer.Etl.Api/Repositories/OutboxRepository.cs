using SqlViewer.Shared.Messages.Etl.Commands;
using SqlViewer.Etl.Core.Data.DbContexts;

namespace SqlViewer.Etl.Api.Repositories;

public class OutboxRepository(EtlDbContext dbContext) : IOutboxRepository
{
    public async Task AddTransferCommandAsync(Guid userUid, Guid correlationId, string topic, string payload)
    {
        await dbContext.OutboxMessages.AddAsync(new()
        {
            UserUid = userUid,
            CorrelationId = correlationId,
            Topic = topic,
            MessageType = nameof(StartDataTransferCommand),
            Payload = payload,
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();
    }
}
