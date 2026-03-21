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
}
