using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Enums;
using SqlViewer.ApiGateway.ViewModels.SqlQueries;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Factories;

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
}
