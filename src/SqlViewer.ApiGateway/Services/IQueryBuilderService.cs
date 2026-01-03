using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.ApiGateway.Services;

public interface IQueryBuilderService
{
    string GetCreateTableQuery(
        VelocipedeDatabaseType databaseType,
        string tableName,
        IEnumerable<VelocipedeColumnInfo> columnInfos);
}
