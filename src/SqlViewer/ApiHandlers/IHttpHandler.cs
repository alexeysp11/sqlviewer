using SqlViewer.Common.Dtos.SqlQueries;

namespace SqlViewer.ApiHandlers;

public interface IHttpHandler : IDisposable
{
    Task<SqlQueryResponseDto> ExecuteQueryAsync(SqlQueryRequestDto requestDto);
}
