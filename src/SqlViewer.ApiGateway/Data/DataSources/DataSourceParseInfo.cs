namespace SqlViewer.ApiGateway.Data.DataSources;

public sealed record DataSourceParseInfo
{
    public required int? Id { get; init; }
    public required string? Name { get; init; }
}
