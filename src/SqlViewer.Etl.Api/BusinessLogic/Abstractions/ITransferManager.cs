namespace SqlViewer.Etl.Api.BusinessLogic.Abstractions;

public interface ITransferManager
{
    Task<Guid> InitiateTransferAsync(Guid correlationId, Guid userUid, string requestJson);
}
