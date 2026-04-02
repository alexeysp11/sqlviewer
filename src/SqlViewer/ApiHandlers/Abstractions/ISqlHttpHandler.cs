using SqlViewer.Shared.Dtos.SqlQueries;

namespace SqlViewer.ApiHandlers.Abstractions;

public interface ISqlHttpHandler
{
    Task<SqlQueryResponseDto> ExecuteQueryAsync(SqlQueryRequestDto requestDto, CancellationToken ct);
}
