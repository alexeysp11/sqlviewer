using SqlViewer.Common.Dtos.Metadata;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Common.Tests.Dtos;

public sealed record TestCaseColumnInfoDto
{
    public required VelocipedeDatabaseType DatabaseType { get; init; }
    public required ColumnInfoDto ColumnInfoDto { get; init; }
}
