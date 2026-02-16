using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.QueryExecution.Services;

public interface ISqlQueryService
{
    Task<List<dynamic>> QueryAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string query);

    Task ExecuteAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string query);

    Task CreateTableAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName,
        IEnumerable<VelocipedeColumnInfo> columnInfos);

    Task DropTableAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName);
}
