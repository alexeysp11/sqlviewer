using Microsoft.EntityFrameworkCore;
using SqlViewer.QueryExecution.Data.Entities;

namespace SqlViewer.QueryExecution.Data.DbContexts;

public sealed class QueryExecutionDbContext(DbContextOptions<QueryExecutionDbContext> options) : DbContext(options)
{
    public DbSet<DataSourceEntity> DataSources { get; set; }
    public DbSet<DataSourcePermissionEntity> DataSourcePermissions { get; set; }
    public DbSet<QueryLogEntity> QueryLogs { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}