using SqlViewer.ApiGateway.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace SqlViewer.ApiGateway.VerticalSlices.Metadata.Services.Implementations;

public sealed class QueryBuilderService(IDbConnectionFactory dbConnectionFactory) : IQueryBuilderService
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<string> GetCreateTableQueryAsync(
        VelocipedeDatabaseType databaseType,
        string tableName,
        IEnumerable<VelocipedeColumnInfo> columnInfos)
    {
        using IVelocipedeDbConnection dbConnection = await _dbConnectionFactory.GetDbConnectionAsync(databaseType);

        IVelocipedeQueryBuilder queryBuilder = dbConnection.GetQueryBuilder();
        string? sql = queryBuilder
            .CreateTable(tableName)
            .WithColumns(columnInfos)
            .ToString();
        if (string.IsNullOrEmpty(sql))
        {
            throw new InvalidOperationException("Query builder generated empty SQL");
        }
        return sql;
    }
}
