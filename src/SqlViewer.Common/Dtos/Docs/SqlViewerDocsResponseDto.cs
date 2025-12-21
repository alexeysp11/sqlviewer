namespace SqlViewer.Common.Dtos.Docs;

/// <summary>
/// Response for getting database provider docs.
/// </summary>
public sealed class SqlViewerDocsResponseDto : BaseSqlViewerResponseDto
{
    public string? Url { get; set; }
}
