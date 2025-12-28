using SqlViewer.ApiHandlers;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Common.Enums;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace SqlViewer.Services;

public sealed class SqlApiService(IHttpHandler httpHandler) : ISqlApiService
{
    private readonly IHttpHandler _httpHandler = httpHandler;

    public async Task<DataTable> QueryAsync(VelocipedeDatabaseType databaseType, string connectionString, string query)
    {
        SqlQueryRequestDto requestDto = new()
        {
            DatabaseType = databaseType,
            ConnectionString = connectionString,
            Query = query
        };

        SqlQueryResponseDto responseDto = await _httpHandler.ExecuteQueryAsync(requestDto);

        if (responseDto is null || responseDto.Status is SqlOperationStatus.None)
            throw new InvalidOperationException("Unable to get response DTO");
        if (responseDto.Status is SqlOperationStatus.Failed)
            throw new InvalidOperationException(responseDto.ErrorMessage);

        return responseDto.QueryResult.ToDataTable();
    }

    public void Dispose() => _httpHandler.Dispose();
}
