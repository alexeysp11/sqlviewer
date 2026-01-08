namespace SqlViewer.ApiGateway.Models;

public sealed record AppSettings
{
    public required string MetadataConnectionString { get; init; }
}
