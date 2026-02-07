using Microsoft.EntityFrameworkCore;
using SqlViewer.ApiGateway.Data.DbContexts;
using SqlViewer.Common.Models;
using SqlViewer.Common.Repositories;
using SqlViewer.Common.Services;

namespace SqlViewer.ApiGateway.VerticalSlices.Metadata.Repositories.Implementations;

public sealed class DataSourceRepository(
    ApiGatewayDbContext dbContext,
    IEncryptionService encryptionService) : IDataSourceRepository
{
    public async Task<string> GetRealConnectionStringAsync(int? dataSourceId, string? dataSourceName)
    {
        if (!dataSourceId.HasValue && string.IsNullOrWhiteSpace(dataSourceName))
        {
            throw new ArgumentException("Please, specify the Id or Name of the data source");
        }

        IQueryable<DataSource> query = dbContext.DataSources.AsNoTracking();
        if (dataSourceId.HasValue)
        {
            query = query.Where(ds => ds.Id == dataSourceId.Value);
        }
        if (!string.IsNullOrWhiteSpace(dataSourceName))
        {
            query = query.Where(ds => ds.Name == dataSourceName);
        }
        DataSource dataSource = await query.FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("Unable to find specified data source");

        return encryptionService.Decrypt(dataSource.EncryptedConnectionString);
    }
}
