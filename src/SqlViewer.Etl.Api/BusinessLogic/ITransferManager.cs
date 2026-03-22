namespace SqlViewer.Etl.Api.BusinessLogic;

public interface ITransferManager
{
    Task<Guid> InitiateTransferAsync(Guid userUid, string requestJson);
}
