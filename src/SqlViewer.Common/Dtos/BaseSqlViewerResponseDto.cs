using System.Text.Json.Serialization;
using SqlViewer.Common.Enums;

namespace SqlViewer.Common.Dtos;

/// <summary>
/// Abstract base response.
/// </summary>
public abstract class BaseSqlViewerResponseDto
{
    [JsonPropertyName("sqlOperationStatus")]
    public required SqlOperationStatus Status { get; init; }

    [JsonPropertyName("errorMessage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; init; }
}
