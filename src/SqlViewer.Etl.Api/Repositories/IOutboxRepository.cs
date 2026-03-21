namespace SqlViewer.Etl.Api.Repositories;

public interface IOutboxRepository
{
    Task AddTransferCommandAsync(Guid correlationId, string topic, string payload);
}
