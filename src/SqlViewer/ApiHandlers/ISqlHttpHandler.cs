using SqlViewer.Common.Dtos.SqlQueries;

namespace SqlViewer.ApiHandlers;

public interface ISqlHttpHandler : IDisposable
{
    Task<SqlQueryResponseDto> ExecuteQueryAsync(SqlQueryRequestDto requestDto);
}
