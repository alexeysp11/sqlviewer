using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Tests.Services.MetadataServiceTests;

public sealed record TestCaseMetadataTables
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required string ConnectionString { get; init; }
}
