using SqlViewer.Common.Dtos.QueryBuilder;
using SqlViewer.Common.Dtos.SqlQueries;

namespace SqlViewer.ApiHandlers;

public interface IQueryBuilderHttpHandler : IDisposable
{
    Task<QueryBuilderResponseDto> GetCreateTableQueryAsync(CreateTableRequestDto requestDto);
}
