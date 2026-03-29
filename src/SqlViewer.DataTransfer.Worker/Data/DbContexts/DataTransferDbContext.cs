using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.Shared.Messages.Storage.Entities;
using Microsoft.EntityFrameworkCore;

namespace SqlViewer.DataTransfer.Worker.Data.DbContexts;

/// <summary>
/// Database context for the Data Transfer microservice.
/// Manages execution states and reliability patterns (Inbox/Outbox).
/// </summary>
public sealed class DataTransferDbContext(DbContextOptions<DataTransferDbContext> options) : DbContext(options)
{
    public DbSet<DataTransferSagaEntity> DataTransferSagas { get; set; }

    /// <summary>
    /// Incoming messages from Kafka to be processed by the worker.
    /// </summary>
    public DbSet<InboxMessageEntity> InboxMessages { get; set; }

    /// <summary>
    /// Current state of data transfer executions.
    /// </summary>
    public DbSet<TransferExecutionEntity> TransferExecutions { get; set; }

    /// <summary>
    /// Outgoing events/notifications to be sent back to the ETL service via Kafka.
    /// </summary>
    public DbSet<OutboxMessageEntity> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InboxMessageEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CorrelationId).IsUnique();
            entity.HasIndex(e => e.Status);
            entity.Property(e => e.Payload).HasColumnType("jsonb");
        });

        modelBuilder.Entity<TransferExecutionEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CorrelationId).IsUnique(false);
            entity.Property(e => e.Progress)
                .HasDefaultValue(0.0);
            entity.Property(e => e.TableName)
                .HasMaxLength(255)
                .IsRequired();
        });

        modelBuilder.Entity<OutboxMessageEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}
