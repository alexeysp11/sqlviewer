using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Services;

public interface IMetadataService
{
    Task<List<string>> GetTablesAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString);
}
