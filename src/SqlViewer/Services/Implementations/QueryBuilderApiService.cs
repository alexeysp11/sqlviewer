using SqlViewer.ApiHandlers;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Common.Dtos.QueryBuilder;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Common.Enums;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class QueryBuilderApiService(IHttpHandler httpHandler) : IQueryBuilderApiService
{
    private readonly IHttpHandler _httpHandler = httpHandler;

    public async Task<string> GetCreateTableQueryAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName,
        IEnumerable<ColumnInfoDto> columnInfos)
    {
        CreateTableRequestDto requestDto = new()
        {
            DatabaseType = databaseType,
            ConnectionString = connectionString,
            TableName = tableName,
            Columns = columnInfos
        };

        QueryBuilderResponseDto responseDto = await _httpHandler.GetCreateTableQueryAsync(requestDto);

        if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
            throw new InvalidOperationException("Unable to get response DTO");
        if (responseDto.Status is SqlOperationStatus.Failed)
            throw new InvalidOperationException(responseDto.ErrorMessage);
        if (string.IsNullOrEmpty(responseDto.Query))
            throw new InvalidOperationException("Generated query could not be null or empty");

        return responseDto.Query;
    }

    public void Dispose() => _httpHandler?.Dispose();
}
