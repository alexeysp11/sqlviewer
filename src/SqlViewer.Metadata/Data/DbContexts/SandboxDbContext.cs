using Microsoft.EntityFrameworkCore;
using SqlViewer.Shared.Seed.Sandbox.Entities.Archive;
using SqlViewer.Shared.Seed.Sandbox.Entities.Public;

namespace SqlViewer.Metadata.Data.DbContexts;

public sealed class SandboxDbContext(DbContextOptions<SandboxDbContext> options) : DbContext(options)
{
    public DbSet<AuditLogEntity> AuditLogs { get; set; }
    public DbSet<AuthorEntity> Authors { get; set; }
    public DbSet<BookAuthorEntity> BookAuthors { get; set; }
    public DbSet<BookEntity> Books { get; set; }
    public DbSet<RegionEntity> Regions { get; set; }

    public DbSet<ArchiveLinkEntity> ArchiveLinks { get; set; }
    public DbSet<DeletedUserRegistryEntity> DeletedUserRegistries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArchiveLinkEntity>()
            .HasKey(al => new { al.SourceId, al.TargetId });
    }
}
