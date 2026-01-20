using System.Text.Json.Serialization;

namespace SqlViewer.Common.Dtos.Metadata;

public sealed record ColumnInfoResponseDto : ColumnInfoDto
{
    /// <summary>
    /// Native column type.
    /// </summary>
    [JsonPropertyName("nativeColumnType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NativeColumnType { get; init; }
}
