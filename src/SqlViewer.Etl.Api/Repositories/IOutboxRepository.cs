namespace SqlViewer.Etl.Api.Repositories;

public interface IOutboxRepository
{
    Task AddTransferCommandAsync(Guid userUid, Guid correlationId, string topic, string payload);
}
