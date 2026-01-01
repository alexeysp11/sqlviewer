namespace SqlViewer.Common.Dtos.Metadata;

/// <summary>
/// Response for getting metadata about tables.
/// </summary>
public sealed class MetadataTablesResponseDto : BaseSqlViewerResponseDto
{
    public IEnumerable<string>? Tables { get; init; }
}
