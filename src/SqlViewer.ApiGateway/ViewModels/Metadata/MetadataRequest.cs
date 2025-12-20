namespace SqlViewer.ApiGateway.ViewModels.Metadata;

/// <summary>
/// Request for getting metadata.
/// </summary>
public sealed class MetadataRequest : BaseSqlViewerRequest
{
    public string? TableName { get; init; }
}
