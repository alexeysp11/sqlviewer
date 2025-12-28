using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Tests.Services.SqlQueryServiceTests;

public sealed record TestCaseQuerying
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required string ConnectionString { get; init; }
    public required string Query { get; init; }
}
