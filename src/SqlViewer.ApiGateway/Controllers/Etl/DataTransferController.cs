using Microsoft.AspNetCore.Mvc;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Etl;
using SqlViewer.Etl;

namespace SqlViewer.ApiGateway.Controllers.Etl;

[ApiController]
public class DataTransferController(EtlTransferService.EtlTransferServiceClient grpcClient) : ControllerBase
{
    [HttpPost(RestApiPaths.Etl.StartDataTransfer)]
    public async Task<IActionResult> Start([FromBody] StartTransferRequestDto dto)
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

    [HttpGet("api/etl/data-transfer/status/{correlationId}")]
    public async Task<IActionResult> GetStatus(Guid correlationId)
    {
        //var status = await grpcClient.GetTransferStatusAsync(new { Id = correlationId });
        //return Ok(status);
        return Ok($"ok: {correlationId}");
    }
}
