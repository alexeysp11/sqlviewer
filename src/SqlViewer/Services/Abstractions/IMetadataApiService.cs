using SqlViewer.Shared.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Abstractions;

public interface IMetadataApiService
{
    Task<IEnumerable<ColumnInfoResponseDto>> GetColumnsAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName,
        CancellationToken ct = default);

    Task<IEnumerable<string>> GetTablesAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        CancellationToken ct = default);
}
