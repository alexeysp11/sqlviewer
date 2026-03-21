using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Extensions;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.SqlQueries;
using SqlViewer.Metadata;
using SqlViewer.QueryExecution;

namespace SqlViewer.ApiGateway.Controllers.QueryExecution;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class SqlApiController(
    ILogger<SqlApiController> logger,
    QueryExecutionService.QueryExecutionServiceClient grpcClient) : ControllerBase
{
    [HttpPost]
    [Route(RestApiPaths.Sql.Query)]
    public async Task<ActionResult<SqlQueryResponseDto>> QueryAsync([FromBody] SqlQueryRequestDto request)
    {
        try
        {
            SqlQueryResponse protoResponse = await grpcClient.QueryAsync(new SqlQueryRequest
            {
                DatabaseType = (DatabaseType)request.DatabaseType,
                ConnectionString = request.ConnectionString,
                Query = request.Query
            });

            List<dynamic> dynamicResult = protoResponse.QueryResult.Select(row => {
                IDictionary<string, object?> expando = new ExpandoObject();
                foreach (KeyValuePair<string, Google.Protobuf.WellKnownTypes.Value> field in row.Fields)
                {
                    expando.Add(field.Key, field.Value.Unwrap());
                }
                return (dynamic)expando;
            }).ToList();

            return Ok(new SqlQueryResponseDto { QueryResult = dynamicResult });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Query failed");
            return StatusCode(500, ex.Message);
        }
    }
}
