namespace SqlViewer.ApiGateway.ViewModels.Metadata;

/// <summary>
/// Request for getting metadata about tables.
/// </summary>
public sealed class MetadataTablesResponse : BaseSqlViewerResponse
{
    public IEnumerable<string>? Tables { get; init; }
}
