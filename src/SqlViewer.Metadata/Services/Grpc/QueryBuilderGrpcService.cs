using Grpc.Core;
using SqlViewer.Metadata.Domain.QueryBuilders;
using SqlViewer.QueryBuilder;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.Metadata.Services.Grpc;

public sealed class QueryBuilderGrpcService(
    ILogger<QueryBuilderGrpcService> logger,
    IQueryBuilderManager queryBuilderManager) : QueryBuilderService.QueryBuilderServiceBase
{
    public override async Task<QueryBuilderResponse> GetCreateTableQuery(CreateTableRequest request, ServerCallContext context)
    {
        try
        {
            IEnumerable<VelocipedeColumnInfo> columnInfos = request.Columns.Select(c => new VelocipedeColumnInfo
            {
                DatabaseType = MapToVelocipedeDatabaseType(request.DatabaseType),
                ColumnName = c.ColumnName,
                ColumnType = (System.Data.DbType)c.ColumnType,
                CharMaxLength = c.CharMaxLength,
                NumericPrecision = c.NumericPrecision,
                NumericScale = c.NumericScale,
                IsPrimaryKey = c.IsPrimaryKey,
                IsNullable = c.IsNullable
            });

            string sqlQuery = await queryBuilderManager.GetCreateTableQueryAsync(
                (VelocipedeDatabaseType)request.DatabaseType,
                request.TableName,
                columnInfos);

            return new QueryBuilderResponse { Query = sqlQuery };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error building create table query");
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    private static VelocipedeDatabaseType MapToVelocipedeDatabaseType(DatabaseType source)
    {
        return source switch
        {
            DatabaseType.None => VelocipedeDatabaseType.None,
            DatabaseType.InMemory => VelocipedeDatabaseType.InMemory,
            DatabaseType.Oracle => VelocipedeDatabaseType.Oracle,
            DatabaseType.Clickhouse => VelocipedeDatabaseType.Clickhouse,
            DatabaseType.Firebird => VelocipedeDatabaseType.Firebird,
            DatabaseType.Sqlite => VelocipedeDatabaseType.SQLite,
            DatabaseType.Postgresql => VelocipedeDatabaseType.PostgreSQL,
            DatabaseType.Mssql => VelocipedeDatabaseType.MSSQL,
            DatabaseType.Mysql => VelocipedeDatabaseType.MySQL,
            DatabaseType.Mariadb => VelocipedeDatabaseType.MariaDB,
            DatabaseType.Hsqldb => VelocipedeDatabaseType.HSQLDB,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, $"The value of enum {nameof(DatabaseType)} is not supported"),
        };
    }
}
