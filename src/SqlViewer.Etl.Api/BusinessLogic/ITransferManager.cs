namespace SqlViewer.Etl.Api.BusinessLogic;

public interface ITransferManager
{
    Task<Guid> InitiateTransferAsync(string requestJson);
}
