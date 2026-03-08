using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Metadata.Domain.QueryBuilders;

public interface IQueryBuilderManager
{
    Task<string> GetCreateTableQueryAsync(
        VelocipedeDatabaseType databaseType,
        string tableName,
        IEnumerable<VelocipedeColumnInfo> columnInfos);
}
