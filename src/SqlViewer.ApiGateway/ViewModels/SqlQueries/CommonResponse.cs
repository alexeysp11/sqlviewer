using SqlViewer.ApiGateway.Enums;

namespace SqlViewer.ApiGateway.ViewModels.SqlQueries;

/// <summary>
/// Common response for executing SQL query.
/// </summary>
public sealed class CommonResponse
{
    public required SqlOperationStatus Status { get; init; }
    public string? ErrorMessage { get; init; }
}
