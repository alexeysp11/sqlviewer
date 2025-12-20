namespace SqlViewer.ApiGateway.ViewModels.Docs;

/// <summary>
/// Response for getting database provider docs.
/// </summary>
public sealed class SqlViewerDocsResponse : BaseSqlViewerResponse
{
    public string? Url { get; set; }
}
