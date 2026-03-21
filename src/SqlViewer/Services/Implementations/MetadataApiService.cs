using SqlViewer.ApiHandlers;
using SqlViewer.Shared.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class MetadataApiService(IMetadataHttpHandler httpHandler) : IMetadataApiService
{
    private readonly IMetadataHttpHandler _httpHandler = httpHandler;

    public async Task<IEnumerable<ColumnInfoResponseDto>> GetColumnsAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName)
    {
        MetadataRequestDto requestDto = new()
        {
            DatabaseType = databaseType,
            ConnectionString = connectionString,
            TableName = tableName
        };
        MetadataColumnsResponseDto responseDto = await _httpHandler.GetColumnsAsync(requestDto);
        return responseDto.Columns ?? [];
    }

    public async Task<IEnumerable<string>> GetTablesAsync(VelocipedeDatabaseType databaseType, string connectionString)
    {
        MetadataRequestDto requestDto = new()
        {
            DatabaseType = databaseType,
            ConnectionString = connectionString,
        };
        MetadataTablesResponseDto responseDto = await _httpHandler.GetTables(requestDto);
        return responseDto.Tables ?? [];
    }

    public void Dispose() => _httpHandler?.Dispose();
}
