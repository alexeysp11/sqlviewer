using SqlViewer.ApiHandlers;
using SqlViewer.Shared.Dtos.Metadata;
using SqlViewer.Shared.Dtos.QueryBuilder;
using SqlViewer.Shared.Dtos.SqlQueries;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Services.Implementations;

public sealed class QueryBuilderApiService(IQueryBuilderHttpHandler httpHandler) : IQueryBuilderApiService
{
    public async Task<string> GetCreateTableQueryAsync(
        VelocipedeDatabaseType databaseType,
        string tableName,
        IEnumerable<ColumnInfoDto> columnInfos)
    {
        CreateTableRequestDto requestDto = new()
        {
            DatabaseType = databaseType,
            ConnectionString = string.Empty,
            TableName = tableName,
            Columns = columnInfos
        };

        QueryBuilderResponseDto responseDto = await httpHandler.GetCreateTableQueryAsync(requestDto);
        if (string.IsNullOrEmpty(responseDto.Query))
            throw new InvalidOperationException("Generated query could not be null or empty");
        return responseDto.Query;
    }

    public void Dispose() => httpHandler?.Dispose();
}
