using SqlViewer.ApiGateway.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace SqlViewer.ApiGateway.Services;

public sealed class SqlQueryService(IDbConnectionFactory dbConnectionFactory) : ISqlQueryService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public Task CreateTableAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName,
        IEnumerable<VelocipedeColumnInfo> columnInfos)
    {
        using IVelocipedeDbConnection dbConnection = _dbConnectionFactory.GetDbConnection(
            databaseType,
            connectionString);

        IVelocipedeQueryBuilder queryBuilder = dbConnection.GetQueryBuilder();
        string? sql = queryBuilder
            .CreateTable(tableName)
            .WithColumns(columnInfos)
            .ToString();
        if (string.IsNullOrEmpty(sql))
        {
            throw new InvalidOperationException("Query builder generated empty SQL");
        }
        return dbConnection.ExecuteAsync(sql);
    }

    public Task DropTableAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName)
    {
        using IVelocipedeDbConnection dbConnection = _dbConnectionFactory.GetDbConnection(
            databaseType,
            connectionString);
        string sql = $@"drop table ""{tableName}""";
        return dbConnection.ExecuteAsync(sql);
    }

    public Task ExecuteAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string query)
    {
        using IVelocipedeDbConnection dbConnection = _dbConnectionFactory.GetDbConnection(
            databaseType,
            connectionString);
        return dbConnection.ExecuteAsync(query);
    }

    public Task<List<dynamic>> QueryAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string query)
    {
        using IVelocipedeDbConnection dbConnection = _dbConnectionFactory.GetDbConnection(
            databaseType,
            connectionString);
        return dbConnection.QueryAsync<dynamic>(query);
    }
}
