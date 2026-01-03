using SqlViewer.Common.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services;

public interface IQueryBuilderApiService : IDisposable
{
    Task<string> GetCreateTableQueryAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName,
        IEnumerable<ColumnInfoDto> columnInfos);
}
