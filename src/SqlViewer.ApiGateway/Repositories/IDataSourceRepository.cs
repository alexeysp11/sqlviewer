namespace SqlViewer.ApiGateway.Repositories;

public interface IDataSourceRepository
{
    Task<string> GetRealConnectionStringAsync(int? dataSourceId, string? dataSourceName);
}
