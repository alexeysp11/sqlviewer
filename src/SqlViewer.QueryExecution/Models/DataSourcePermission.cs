using SqlViewer.QueryExecution.Enums;

namespace SqlViewer.QueryExecution.Models;

public sealed record DataSourcePermission
{
    public long Id { get; set; }
    public Guid Uid { get; set; }
    public User User { get; set; } = null!;
    public DataSource DataSource { get; set; } = null!;
    public AccessLevelType AccessLevel { get; set; }
}
