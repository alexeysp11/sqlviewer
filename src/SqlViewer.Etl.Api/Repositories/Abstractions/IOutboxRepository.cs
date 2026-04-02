namespace SqlViewer.Etl.Api.Repositories.Abstractions;

public interface IOutboxRepository
{
    Task AddTransferCommandAsync(Guid userUid, Guid correlationId, string topic, string payload);
}
