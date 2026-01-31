namespace SqlViewer.Models;

public sealed record AppSettings
{
    public required string ServerScheme { get; init; }
    public required string ServerHost { get; init; }
    public required int ServerPort { get; init; }
    public required int HttpClientTimeoutSeconds { get; init; }
}
