using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Messages.Storage.Entities;
using SqlViewer.Etl.Core.Data.Entities;

namespace SqlViewer.Etl.Core.Data.DbContexts;

public class EtlDbContext(DbContextOptions<EtlDbContext> options) : DbContext(options)
{
    public DbSet<DataTransferSagaStateEntity> DataTransferSagaStates { get; set; }
    public DbSet<InboxMessageEntity> InboxMessages { get; set; }
    public DbSet<OutboxMessageEntity> OutboxMessages { get; set; }
}
