using SqlViewer.Common.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace SqlViewer.QueryExecution.Domain.SqlQuery;

public sealed class SqlQueryManager(IDbConnectionFactory dbConnectionFactory) : ISqlQueryManager
{
    public async Task CreateTableAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName,
        IEnumerable<VelocipedeColumnInfo> columnInfos)
    {
        using IVelocipedeDbConnection dbConnection = await dbConnectionFactory.GetDbConnectionAsync(
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
        await dbConnection.ExecuteAsync(sql);
    }

    public async Task DropTableAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string tableName)
    {
        using IVelocipedeDbConnection dbConnection = await dbConnectionFactory.GetDbConnectionAsync(
            databaseType,
            connectionString);
        string sql = $@"drop table ""{tableName}""";
        await dbConnection.ExecuteAsync(sql);
    }

    public async Task ExecuteAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string query)
    {
        using IVelocipedeDbConnection dbConnection = await dbConnectionFactory.GetDbConnectionAsync(
            databaseType,
            connectionString);
        await dbConnection.ExecuteAsync(query);
    }

    public async Task<List<dynamic>> QueryAsync(
        VelocipedeDatabaseType databaseType,
        string connectionString,
        string query)
    {
        using IVelocipedeDbConnection dbConnection = await dbConnectionFactory.GetDbConnectionAsync(
            databaseType,
            connectionString);
        return await dbConnection.QueryAsync<dynamic>(query);
    }
}
