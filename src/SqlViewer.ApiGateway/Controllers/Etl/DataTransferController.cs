using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Controllers.Metadata;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Etl;
using SqlViewer.Etl;

namespace SqlViewer.ApiGateway.Controllers.Etl;

[ApiController]
public class DataTransferController(
    ILogger<MetadataApiController> logger,
    EtlTransferService.EtlTransferServiceClient grpcClient) : ControllerBase
{
    [HttpPost(RestApiPaths.Etl.StartDataTransfer)]
    public async Task<IActionResult> Start([FromBody] StartTransferRequestDto dto)
    {
        try
        {
            StartTransferRequest grpcRequest = new()
            {
                SourceConnectionString = dto.SourceConnectionString,
                TargetConnectionString = dto.TargetConnectionString,
                TableName = dto.TableName
            };

            StartTransferResponse response = await grpcClient.StartTransferAsync(grpcRequest);

            return Ok(new StartTransferResponseDto
            {
                CorrelationId = Guid.Parse(response.CorrelationId),
                CreatedAt = response.CreatedAt.ToDateTime(),
                Message = response.Message
            });
        }
        catch (Exception ex)
        {
            logger.LogError("Unable to start transfer: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("api/etl/data-transfer/status/{correlationId}")]
    public async Task<IActionResult> GetStatus(Guid correlationId)
    {
        //var status = await grpcClient.GetTransferStatusAsync(new { Id = correlationId });
        //return Ok(status);
        return Ok($"ok: {correlationId}");
    }
}
