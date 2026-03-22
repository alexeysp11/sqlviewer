using Microsoft.EntityFrameworkCore;
using SqlViewer.Shared.Messages.Storage.Entities;
using SqlViewer.Etl.Core.Data.Entities;

namespace SqlViewer.Etl.Core.Data.DbContexts;

public class EtlDbContext(DbContextOptions<EtlDbContext> options) : DbContext(options)
{
    public DbSet<DataTransferSagaStateEntity> DataTransferSagaStates { get; set; }
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

        // Hash index on CorrelationId (Postgres only).
        modelBuilder.Entity<TransferJobEntity>()
            .HasIndex(j => j.CorrelationId)
            .HasDatabaseName("IX_TransferJobs_CorrelationId")
            .HasMethod("hash");
    }
}
