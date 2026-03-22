using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Controllers.Metadata;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.Etl;
using Microsoft.AspNetCore.Authorization;

namespace SqlViewer.ApiGateway.Controllers.Etl;

[ApiController]
[Authorize]
public class DataTransferController(
    ILogger<MetadataApiController> logger,
    EtlTransferService.EtlTransferServiceClient grpcClient) : ControllerBase
{
    private const int MaxDataSize = 100;

    [HttpPost(RestApiPaths.Etl.DataTransfer.Start)]
    public async Task<IActionResult> Start([FromBody] StartTransferRequestDto dto)
    {
        try
        {
            StartTransferRequest grpcRequest = new()
            {
                SourceConnectionString = dto.SourceConnectionString,
                TargetConnectionString = dto.TargetConnectionString,
                TableName = dto.TableName,
                UserUid = dto.UserUid,
            };

            StartTransferResponse response = await grpcClient.StartTransferAsync(grpcRequest);

            return Ok(new StartTransferResponseDto
            {
                CorrelationId = Guid.Parse(response.CorrelationId),
                CreatedAt = response.CreatedAt.ToDateTime(),
                Message = response.Message,
            });
        }
        catch (Exception ex)
        {
            logger.LogError("Unable to start transfer: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet(RestApiPaths.Etl.DataTransfer.GetStatus)]
    public async Task<IActionResult> GetStatus(Guid userUid)
    {
        //var status = await grpcClient.GetTransferStatusAsync(new { Id = userUid });
        //return Ok(status);
        return Ok($"ok: {userUid}");
    }

    [HttpGet(RestApiPaths.Etl.DataTransfer.GetHistory)]
    public async Task<IActionResult> GetHistory(
        [FromRoute] Guid userUid,
        [FromQuery(Name = "cursor")] DateTime lastCreatedAt,
        [FromQuery(Name = "limit")] int limit)
    {
        if (limit > MaxDataSize)
            limit = MaxDataSize;

        //var status = await grpcClient.GetTransferStatusAsync(new { Id = userUid });
        //return Ok(status);
        return Ok($"userUid: {userUid}, cursor: {lastCreatedAt}, limit: {limit}");
    }
}
