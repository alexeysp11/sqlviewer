using SqlViewer.Common.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services;

public interface IMetadataApiService : IDisposable
{
    Task<IEnumerable<ColumnInfoResponseDto>> GetColumnsAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName);

    Task<IEnumerable<string>> GetTablesAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString);
}
