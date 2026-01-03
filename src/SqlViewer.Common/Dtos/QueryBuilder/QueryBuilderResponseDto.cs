namespace SqlViewer.Common.Dtos.QueryBuilder;

public sealed class QueryBuilderResponseDto : BaseSqlViewerResponseDto
{
    public string? Query { get; init; }
}
