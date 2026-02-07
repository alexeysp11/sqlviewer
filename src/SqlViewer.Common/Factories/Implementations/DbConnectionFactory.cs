using SqlViewer.ApiGateway.Data.DataSources;
using SqlViewer.Common.Parsers.DataSources;
using SqlViewer.Common.Repositories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Factories;

namespace SqlViewer.Common.Factories.Implementations;

public sealed class DbConnectionFactory(IDataSourceRepository dataSourceRepository) : IDbConnectionFactory
{
    public async Task<IVelocipedeDbConnection> GetDbConnectionAsync(VelocipedeDatabaseType databaseType, string? connectionString = null)
    {
        DataSourceParseInfo? dataSourceInfo = DataSourceParser.Parse(connectionString);
        string? actualConnectionString = dataSourceInfo is not null
            ? await dataSourceRepository.GetRealConnectionStringAsync(dataSourceInfo.Id, dataSourceInfo.Name)
            : connectionString;

        return VelocipedeDbConnectionFactory.InitializeDbConnection(databaseType, actualConnectionString);
    }
}
