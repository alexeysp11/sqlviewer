namespace SqlViewer.Common.Dtos.Metadata;

public sealed record ColumnInfoResponseDto : ColumnInfoDto
{
    /// <summary>
    /// Native column type.
    /// </summary>
    public string? NativeColumnType { get; init; }
}
