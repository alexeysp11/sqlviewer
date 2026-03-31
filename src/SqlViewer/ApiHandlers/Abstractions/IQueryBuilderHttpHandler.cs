using SqlViewer.Shared.Dtos.QueryBuilder;
using SqlViewer.Shared.Dtos.SqlQueries;

namespace SqlViewer.ApiHandlers.Abstractions;

public interface IQueryBuilderHttpHandler
{
    Task<QueryBuilderResponseDto> GetCreateTableQueryAsync(CreateTableRequestDto requestDto, CancellationToken ct);
}
