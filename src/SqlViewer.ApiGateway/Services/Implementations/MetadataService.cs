using SqlViewer.ApiGateway.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.ApiGateway.Services.Implementations;

public sealed class MetadataService(IDbConnectionFactory dbConnectionFactory) : IMetadataService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<List<VelocipedeNativeColumnInfo>> GetColumnsAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName)
    {
        using IVelocipedeDbConnection dbConnection = await _dbConnectionFactory.GetDbConnectionAsync(
            databaseType,
            connectionString);
        return await dbConnection.GetColumnsAsync(tableName);
    }

    public async Task<List<string>> GetTablesAsync(VelocipedeDatabaseType databaseType, string connectionString)
    {
        using IVelocipedeDbConnection dbConnection = await _dbConnectionFactory.GetDbConnectionAsync(
            databaseType,
            connectionString);
        return await dbConnection.GetTablesInDbAsync();
    }
}
