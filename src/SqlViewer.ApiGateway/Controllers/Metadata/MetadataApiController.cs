using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Metadata;
using SqlViewer.Metadata;
using SqlViewer.ApiGateway.Mappings;

namespace SqlViewer.ApiGateway.Controllers.Metadata;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class MetadataApiController(
    ILogger<MetadataApiController> logger,
    MetadataService.MetadataServiceClient grpcClient) : ControllerBase
{
    [HttpPost]
    [Route(RestApiPaths.Metadata.Tables)]
    public async Task<ActionResult<MetadataTablesResponseDto>> GetTablesAsync([FromBody] MetadataRequestDto request)
    {
        try
        {
            MetadataRequest protoRequest = request.MapToProto();
            MetadataTablesResponse protoResponse = await grpcClient.GetTablesAsync(protoRequest);

            return Ok(protoResponse.MapToDto());
        }
        catch (Exception ex)
        {
            logger.LogError("Unable to get tables in {DatabaseType}: {Message}", request.DatabaseType, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Metadata.Columns)]
    public async Task<ActionResult<MetadataColumnsResponseDto>> GetColumnsAsync([FromBody] MetadataRequestDto request)
    {
        try
        {
            MetadataRequest protoRequest = request.MapToProto();
            MetadataColumnsResponse protoResponse = await grpcClient.GetColumnsAsync(protoRequest);

            return Ok(protoResponse.MapToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(
                "Unable to get table columns for '{TableName}' in {DatabaseType}: {Message}",
                request.TableName,
                request.DatabaseType,
                ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
