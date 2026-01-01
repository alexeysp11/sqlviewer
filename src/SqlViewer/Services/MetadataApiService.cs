using SqlViewer.ApiHandlers;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Common.Enums;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services;

public sealed class MetadataApiService(IHttpHandler httpHandler) : IMetadataApiService
{
    private readonly IHttpHandler _httpHandler = httpHandler;

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

        if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
            throw new InvalidOperationException("Unable to get response DTO");
        if (responseDto.Status is SqlOperationStatus.Failed)
            throw new InvalidOperationException(responseDto.ErrorMessage);

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

        if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
            throw new InvalidOperationException("Unable to get response DTO");
        if (responseDto.Status is SqlOperationStatus.Failed)
            throw new InvalidOperationException(responseDto.ErrorMessage);

        return responseDto.Tables ?? [];
    }

    public void Dispose() => _httpHandler?.Dispose();
}
