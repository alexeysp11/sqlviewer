using Microsoft.EntityFrameworkCore;
using SqlViewer.Metadata.Data.Entities;

namespace SqlViewer.Metadata.Data.DbContexts;

public sealed class MetadataDbContext(DbContextOptions<MetadataDbContext> options) : DbContext(options)
{
    public DbSet<DataSourceEntity> DataSources { get; set; }
    public DbSet<DataSourcePermissionEntity> DataSourcePermissions { get; set; }
    public DbSet<QueryLogEntity> QueryLogs { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}