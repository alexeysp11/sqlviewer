using SqlViewer.Shared.Dtos.SqlQueries;

namespace SqlViewer.ApiHandlers;

public interface ISqlHttpHandler : IDisposable
{
    Task<SqlQueryResponseDto> ExecuteQueryAsync(SqlQueryRequestDto requestDto);
}
