using Microsoft.EntityFrameworkCore;
using SqlViewer.Security.Models;

namespace SqlViewer.Security.Data.DbContexts;

public sealed class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : DbContext(options)
{
    public DbSet<QueryLog> QueryLogs { get; set; }
    public DbSet<User> Users { get; set; }
}