using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Models;

namespace SqlViewer.ApiGateway.DbContexts;

public sealed class ApiGatewayDbContext(DbContextOptions<ApiGatewayDbContext> options) : DbContext(options)
{
    public DbSet<DataSource> DataSources { get; set; }
    public DbSet<DataSourcePermission> DataSourcePermissions { get; set; }
    public DbSet<QueryLog> QueryLogs { get; set; }
    public DbSet<User> Users { get; set; }
}