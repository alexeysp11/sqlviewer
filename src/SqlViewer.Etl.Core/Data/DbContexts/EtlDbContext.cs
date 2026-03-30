using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Core.Data.DbContexts;

public sealed class EtlDbContext(DbContextOptions<EtlDbContext> options) : DbContext(options)
{
    public DbSet<TransferJobEntity> TransferJobs { get; set; }
    public DbSet<TransferStatusLogEntity> TransferStatusLogs { get; set; }
    public DbSet<InboxMessageEntity> InboxMessages { get; set; }
    public DbSet<OutboxMessageEntity> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // To optimize ORDER BY DESC for CreatedAt and Id.
        modelBuilder.Entity<TransferJobEntity>()
            .HasIndex(j => new { j.CreatedAt, j.Id })
            .HasDatabaseName("IX_TransferJobs_CreatedAt_Id")
            .IsDescending(true, true);

        modelBuilder.Entity<InboxMessageEntity>(entity =>
        {
            // Unique constraint is mandatory for the Idempotent Consumer pattern.
            // It prevents race conditions at the database level.
            entity.HasIndex(m => m.MessageId)
                .IsUnique()
                .HasDatabaseName("UX_InboxMessages_MessageId");

            // Hash index for fast lookups by CorrelationId (Postgres-specific).
            entity.HasIndex(m => m.CorrelationId)
                .HasMethod("hash")
                .HasDatabaseName("IX_InboxMessages_CorrelationId_Hash");
        });
    }
}
