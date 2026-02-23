using SqlViewer.Metadata.Enums;

namespace SqlViewer.Metadata.Models;

public sealed record DataSourcePermission
{
    public long Id { get; set; }
    public User User { get; set; } = null!;
    public DataSource DataSource { get; set; } = null!;
    public AccessLevelType AccessLevel { get; set; }
}
