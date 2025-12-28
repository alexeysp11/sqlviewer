using SqlViewer.ApiGateway.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Services;

public sealed class MetadataService(IDbConnectionFactory dbConnectionFactory) : IMetadataService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public Task<List<string>> GetTablesAsync(VelocipedeDatabaseType databaseType, string connectionString)
    {
        using IVelocipedeDbConnection dbConnection = _dbConnectionFactory.GetDbConnection(
            databaseType,
            connectionString);
        return dbConnection.GetTablesInDbAsync();
    }
}
