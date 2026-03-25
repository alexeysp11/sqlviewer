namespace SqlViewer.Etl.Api.BusinessLogic;

public interface ITransferManager
{
    Task<Guid> InitiateTransferAsync(Guid correlationId, Guid userUid, string requestJson);
}
