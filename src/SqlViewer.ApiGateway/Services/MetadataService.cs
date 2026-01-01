using SqlViewer.ApiGateway.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.ApiGateway.Services;

public sealed class MetadataService(IDbConnectionFactory dbConnectionFactory) : IMetadataService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public Task<List<VelocipedeNativeColumnInfo>> GetColumnsAsync(VelocipedeDatabaseType databaseType, string connectionString, string tableName)
    {
        using IVelocipedeDbConnection dbConnection = _dbConnectionFactory.GetDbConnection(
            databaseType,
            connectionString);
        return dbConnection.GetColumnsAsync(tableName);
    }

    public Task<List<string>> GetTablesAsync(VelocipedeDatabaseType databaseType, string connectionString)
    {
        using IVelocipedeDbConnection dbConnection = _dbConnectionFactory.GetDbConnection(
            databaseType,
            connectionString);
        return dbConnection.GetTablesInDbAsync();
    }
}
