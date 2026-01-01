using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.ApiGateway.Services;

public interface IMetadataService
{
    Task<List<VelocipedeNativeColumnInfo>> GetColumnsAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName);

    Task<List<string>> GetTablesAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString);
}
