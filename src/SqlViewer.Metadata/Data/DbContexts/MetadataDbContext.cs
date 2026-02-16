using Microsoft.EntityFrameworkCore;
using SqlViewer.Metadata.Models;

namespace SqlViewer.Metadata.Data.DbContexts;

public sealed class MetadataDbContext(DbContextOptions<MetadataDbContext> options) : DbContext(options)
{
    public DbSet<DataSource> DataSources { get; set; }
    public DbSet<DataSourcePermission> DataSourcePermissions { get; set; }
    public DbSet<QueryLog> QueryLogs { get; set; }
    public DbSet<User> Users { get; set; }
}