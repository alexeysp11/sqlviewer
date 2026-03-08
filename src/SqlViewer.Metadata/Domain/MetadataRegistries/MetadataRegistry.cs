using SqlViewer.Common.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Metadata.Domain.MetadataRegistries;

public sealed class MetadataRegistry(IDbConnectionFactory dbConnectionFactory) : IMetadataRegistry
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
