using SqlViewer.Shared.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Shared.Tests.Dtos;

public sealed record TestCaseColumnInfoDto
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required ColumnInfoDto ColumnInfoDto { get; init; }
}
