using System.Text.Json.Serialization;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Common.Dtos;

/// <summary>
/// Abstract base request.
/// </summary>
public abstract class BaseSqlViewerRequestDto
{
    [JsonPropertyName("databaseType")]
    public required VelocipedeDatabaseType DatabaseType { get; init; }

    [JsonPropertyName("connectionString")]
    public required string ConnectionString { get; init; }
}
