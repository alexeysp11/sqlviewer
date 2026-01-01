using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Tests.Services.MetadataServiceTests;

public sealed record TestCaseGetMetadata
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required string ConnectionString { get; init; }
    public string? TableName { get; init; }
}
