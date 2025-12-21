namespace SqlViewer.Common.Dtos.Metadata;

/// <summary>
/// Request for getting metadata.
/// </summary>
public sealed class MetadataRequestDto : BaseSqlViewerRequestDto
{
    public string? TableName { get; init; }
}
