using SqlViewer.ApiHandlers;
using SqlViewer.Shared.Dtos.SqlQueries;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace SqlViewer.Services.Implementations;

public sealed class SqlApiService(ISqlHttpHandler httpHandler) : ISqlApiService
{
    public async Task<DataTable> QueryAsync(VelocipedeDatabaseType databaseType, string connectionString, string query)
    {
        SqlQueryRequestDto requestDto = new()
        {
            DatabaseType = databaseType,
            ConnectionString = connectionString,
            Query = query
        };
        SqlQueryResponseDto responseDto = await httpHandler.ExecuteQueryAsync(requestDto)
            ?? throw new InvalidOperationException("Unable to get response DTO");
        return responseDto.QueryResult.ToDataTable();
    }

    public void Dispose() => httpHandler?.Dispose();
}
