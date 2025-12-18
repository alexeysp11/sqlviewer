using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Enums;
using SqlViewer.ApiGateway.ViewModels.SqlQueries;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class SqlApiController : ControllerBase
{
    private readonly ILogger<SqlApiController> _logger;

    public SqlApiController(ILogger<SqlApiController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("/api/sql/query")]
    public async Task<SqlQueryResponse> QueryAsync([FromBody] SqlQueryRequest request)
    {
        try
        {
            using IVelocipedeDbConnection dbConnection
                = VelocipedeDbConnectionFactory.InitializeDbConnection(request.DatabaseType, request.ConnectionString);
            List<dynamic> result = await dbConnection.QueryAsync<dynamic>(request.Query);
            return new() { Status = SqlOperationStatus.Success, QueryResult = result, };
        }
        catch (Exception ex)
        {
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }

    [HttpPost]
    [Route("/api/sql/execute")]
    public async Task<SqlQueryResponse> ExecuteAsync([FromBody] SqlQueryRequest request)
    {
        try
        {
            using IVelocipedeDbConnection dbConnection
                = VelocipedeDbConnectionFactory.InitializeDbConnection(request.DatabaseType, request.ConnectionString);
            await dbConnection.ExecuteAsync(request.Query);
            return new() { Status = SqlOperationStatus.Success, };
        }
        catch (Exception ex)
        {
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }

    [HttpPost]
    [Route("/api/sql/create-table")]
    public async Task<CommonResponse> CreateTableAsync([FromBody] CreateTableRequest request)
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

            return new() { Status = SqlOperationStatus.Success, };
        }
        catch (Exception ex)
        {
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }

    [HttpPost]
    [Route("/api/sql/drop-table")]
    public async Task<CommonResponse> DropTableAsync([FromBody] DropTableRequest request)
    {
        try
        {
            using IVelocipedeDbConnection dbConnection
                = VelocipedeDbConnectionFactory.InitializeDbConnection(request.DatabaseType, request.ConnectionString);

            IVelocipedeQueryBuilder queryBuilder = dbConnection.GetQueryBuilder();
            string sql = $@"drop table ""{request.TableName}""";
            await dbConnection.ExecuteAsync(sql);

            return new() { Status = SqlOperationStatus.Success, };
        }
        catch (Exception ex)
        {
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }
}
