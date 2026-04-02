using System.Data;
using SqlViewer.ApiHandlers.Abstractions;
using SqlViewer.Services.Abstractions;
using SqlViewer.Shared.Dtos.SqlQueries;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace SqlViewer.Services.Implementations;

public sealed class SqlApiService(ISqlHttpHandler httpHandler) : ISqlApiService
{
    public async Task<DataTable> QueryAsync(VelocipedeDatabaseType databaseType, string connectionString, string query, CancellationToken ct = default)
    {
        SqlQueryRequestDto requestDto = new()
        {
            DatabaseType = databaseType,
            ConnectionString = connectionString,
            Query = query
        };
        SqlQueryResponseDto responseDto = await httpHandler.ExecuteQueryAsync(requestDto, ct)
            ?? throw new InvalidOperationException("Unable to get response DTO");
        return responseDto.QueryResult.ToDataTable();
    }
}
