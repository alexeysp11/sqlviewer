using SqlViewer.Common.Enums;

namespace SqlViewer.Common.Dtos;

/// <summary>
/// Abstract base response.
/// </summary>
public abstract class BaseSqlViewerResponseDto
{
    public required SqlOperationStatus Status { get; init; }
    public string? ErrorMessage { get; init; }
}
