using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Enums;
using SqlViewer.ApiGateway.ViewModels.SqlQueries;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class SqlQueryController : ControllerBase
{
    private readonly ILogger<SqlQueryController> _logger;

    public SqlQueryController(ILogger<SqlQueryController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("/api/sql/execute")]
    public async Task<SqlQueryExecuteResponse> ExecuteSqlQueryAsync([FromBody] SqlQueryExecuteRequest request)
    {
        try
        {
            using IVelocipedeDbConnection dbConnection
                = VelocipedeDbConnectionFactory.InitializeDbConnection(request.DatabaseType, request.ConnectionString);
            List<dynamic> result = await dbConnection.QueryAsync<dynamic>(request.Query);
            return new() { Status = SqlQueryExecuteStatus.Success, QueryResult = result, };
        }
        catch (Exception ex)
        {
            return new() { Status = SqlQueryExecuteStatus.Failed, ErrorMessage = ex.Message, };
        }
    }

    [HttpPost]
    [Route("/api/sql/create-table")]
    public async Task<SqlQueryCommonResponse> CreateTableAsync([FromBody] SqlQueryCreateTableRequest request)
    {
        try
        {
            using IVelocipedeDbConnection dbConnection
                = VelocipedeDbConnectionFactory.InitializeDbConnection(request.DatabaseType, request.ConnectionString);
            
            IVelocipedeQueryBuilder queryBuilder = dbConnection.GetQueryBuilder();
            string? sql = queryBuilder
                .CreateTable(request.TableName)
                .WithColumns(request.Columns)
                .ToString();
            if (string.IsNullOrEmpty(sql))
            {
                throw new InvalidOperationException("Query builder generated empty SQL");
            }
            await dbConnection.ExecuteAsync(sql);

            return new() { Status = SqlQueryExecuteStatus.Success, };
        }
        catch (Exception ex)
        {
            return new() { Status = SqlQueryExecuteStatus.Failed, ErrorMessage = ex.Message, };
        }
    }
}
