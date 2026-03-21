using SqlViewer.Shared.Dtos.QueryBuilder;
using SqlViewer.Shared.Dtos.SqlQueries;

namespace SqlViewer.ApiHandlers;

public interface IQueryBuilderHttpHandler : IDisposable
{
    Task<QueryBuilderResponseDto> GetCreateTableQueryAsync(CreateTableRequestDto requestDto);
}
