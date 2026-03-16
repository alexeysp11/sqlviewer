using Microsoft.AspNetCore.Mvc;

namespace SqlViewer.ApiGateway.Controllers.Etl;

[ApiController]
public class EtlDataTransferController : ControllerBase
{
    [HttpGet("api/data-transfer/status/{correlationId}")]
    public async Task<IActionResult> GetStatus(Guid correlationId)
    {
        //var status = await _etlGrpcClient.GetTransferStatusAsync(new { Id = correlationId });
        //return Ok(status);
        return Ok($"ok: {correlationId}");
    }
}
