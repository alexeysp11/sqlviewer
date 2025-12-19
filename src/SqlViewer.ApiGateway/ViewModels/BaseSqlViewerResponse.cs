using SqlViewer.ApiGateway.Enums;

namespace SqlViewer.ApiGateway.ViewModels;

/// <summary>
/// Abstract base response.
/// </summary>
public abstract class BaseSqlViewerResponse
{
    public required SqlOperationStatus Status { get; init; }
    public string? ErrorMessage { get; init; }
}
