using SqlViewer.Shared.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Abstractions;

public interface IQueryBuilderApiService
{
    Task<string> GetCreateTableQueryAsync(
        VelocipedeDatabaseType databaseType,
        string tableName,
        IEnumerable<ColumnInfoDto> columnInfos,
        CancellationToken ct = default);
}
