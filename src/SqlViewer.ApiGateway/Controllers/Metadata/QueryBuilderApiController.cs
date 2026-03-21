using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Mappings;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.QueryBuilder;
using SqlViewer.Shared.Dtos.SqlQueries;
using SqlViewer.QueryBuilder;

namespace SqlViewer.ApiGateway.Controllers.Metadata;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class QueryBuilderApiController(
    ILogger<QueryBuilderApiController> logger,
    QueryBuilderService.QueryBuilderServiceClient grpcClient) : ControllerBase
{
    [HttpPost]
    [Route(RestApiPaths.QueryBuilder.CreateTable)]
    public async Task<ActionResult<QueryBuilderResponseDto>> GetCreateTableQueryAsync([FromBody] CreateTableRequestDto request)
    {
        try
        {
            CreateTableRequest protoRequest = request.MapToProto();
            QueryBuilderResponse protoResponse = await grpcClient.GetCreateTableQueryAsync(protoRequest);
            return Ok(new QueryBuilderResponseDto { Query = protoResponse.Query });
        }
        catch (RpcException ex)
        {
            logger.LogError("gRPC call failed: {Status}", ex.Status);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in QueryBuilder");
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
