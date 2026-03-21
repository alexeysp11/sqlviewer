using Microsoft.EntityFrameworkCore;
using SqlViewer.Shared.Repositories;
using SqlViewer.Shared.Services;
using SqlViewer.QueryExecution.Data.DbContexts;
using SqlViewer.QueryExecution.Data.Entities;

namespace SqlViewer.QueryExecution.Repositories.Implementations;

public sealed class DataSourceRepository(
    QueryExecutionDbContext dbContext,
    IEncryptionService encryptionService) : IDataSourceRepository
{
    public async Task<string> GetRealConnectionStringAsync(int? dataSourceId, string? dataSourceName)
    {
        if (!dataSourceId.HasValue && string.IsNullOrWhiteSpace(dataSourceName))
        {
            throw new ArgumentException("Please, specify the Id or Name of the data source");
        }

        IQueryable<DataSourceEntity> query = dbContext.DataSources.AsNoTracking();
        if (dataSourceId.HasValue)
        {
            query = query.Where(ds => ds.Id == dataSourceId.Value);
        }
        if (!string.IsNullOrWhiteSpace(dataSourceName))
        {
            query = query.Where(ds => ds.Name == dataSourceName);
        }
        DataSourceEntity dataSource = await query.FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("Unable to find specified data source");

        return encryptionService.Decrypt(dataSource.EncryptedConnectionString);
    }
}
