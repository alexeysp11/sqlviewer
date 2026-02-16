using Microsoft.EntityFrameworkCore;
using SqlViewer.QueryExecution.Models;

namespace SqlViewer.QueryExecution.Data.DbContexts;

public sealed class QueryExecutionDbContext(DbContextOptions<QueryExecutionDbContext> options) : DbContext(options)
{
    public DbSet<DataSource> DataSources { get; set; }
    public DbSet<DataSourcePermission> DataSourcePermissions { get; set; }
    public DbSet<QueryLog> QueryLogs { get; set; }
    public DbSet<User> Users { get; set; }
}