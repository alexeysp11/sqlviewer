using SqlViewer.QueryExecution.Enums;

namespace SqlViewer.QueryExecution.Data.Entities;

public sealed record DataSourcePermissionEntity
{
    public long Id { get; set; }
    public Guid Uid { get; set; }
    public UserEntity User { get; set; } = null!;
    public DataSourceEntity DataSource { get; set; } = null!;
    public AccessLevelType AccessLevel { get; set; }
}
