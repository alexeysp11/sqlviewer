namespace SqlViewer.Tests.Infrastructure.Models;

public sealed record SimpleTableRecord
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? AdditionalInfo { get; init; }
}
