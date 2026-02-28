using Microsoft.EntityFrameworkCore;
using SqlViewer.Security.Data.Entities;

namespace SqlViewer.Security.Data.DbContexts;

public sealed class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : DbContext(options)
{
    public DbSet<QueryLogEntity> QueryLogs { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}