using Microsoft.EntityFrameworkCore;
using SqlViewer.Common.Messages.Common.Entities;
using SqlViewer.Etl.Data.Entities;

namespace SqlViewer.Etl.Data.DbContexts;

public class EtlDbContext(DbContextOptions<EtlDbContext> options) : DbContext(options)
{
    public DbSet<DataTransferSagaStateEntity> DataTransferSagaStates { get; set; }
    public DbSet<InboxMessageEntity> InboxMessages { get; set; }
    public DbSet<OutboxMessageEntity> OutboxMessages { get; set; }
}
