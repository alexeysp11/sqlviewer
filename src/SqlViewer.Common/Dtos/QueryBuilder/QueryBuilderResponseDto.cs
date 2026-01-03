namespace SqlViewer.Common.Dtos.QueryBuilder;

public sealed class QueryBuilderResponseDto : BaseSqlViewerResponseDto
{
    public required string Query { get; init; }
}
