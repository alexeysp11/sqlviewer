using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.ApiGateway.Services;

public interface IQueryBuilderService
{
    Task<string> GetCreateTableQueryAsync(
        VelocipedeDatabaseType databaseType,
        string tableName,
        IEnumerable<VelocipedeColumnInfo> columnInfos);
}
