using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Controllers.Metadata;
using SqlViewer.Etl;
using SqlViewer.Etl.Api.Mappings;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.ApiGateway.Controllers.Etl;

[ApiController]
[Authorize]
public class DataTransferController(
    ILogger<MetadataApiController> logger,
    EtlTransferService.EtlTransferServiceClient grpcClient) : ControllerBase
{
    private const int MinDataSize = 20;
    private const int MaxDataSize = 100;

    [HttpPost(RestApiPaths.Etl.DataTransfer.Start)]
    public async Task<ActionResult<StartTransferResponseDto>> Start([FromBody] StartTransferRequestDto dto)
    {
        try
        {
            StartTransferRequest grpcRequest = new()
            {
                SourceConnectionString = dto.SourceConnectionString,
                TargetConnectionString = dto.TargetConnectionString,
                SourceDatabaseType = EtlMapper.MapToDatabaseType(dto.SourceDatabaseType),
                TargetDatabaseType = EtlMapper.MapToDatabaseType(dto.TargetDatabaseType),
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
    public async Task<ActionResult<TransferHistoryResponseDto>> GetHistory(
    [FromRoute] Guid userUid,
    [FromQuery] Guid? correlationId,
    [FromQuery] int limit = MaxDataSize)
    {
        if (limit <= 0) limit = MinDataSize;
        if (limit > MaxDataSize) limit = MaxDataSize;

        try
        {
            GetTransferHistoryRequest request = new()
            {
                UserUid = userUid.ToString(),
                CursorCorrelationId = correlationId?.ToString() ?? string.Empty,
                Limit = limit
            };

            GetTransferHistoryResponse grpcResponse = await grpcClient.GetTransferHistoryAsync(request);
            TransferHistoryResponseDto result = new()
            {
                CursorCorrelationId = Guid.TryParse(grpcResponse.CursorCorrelationId, out Guid nextGuid)
                    ? nextGuid
                    : null,
                Items = grpcResponse.Items.Select(item => new TransferJobDto
                {
                    CorrelationId = Guid.Parse(item.CorrelationId),
                    SourceConnectionString = item.SourceConnectionString,
                    TargetConnectionString = item.TargetConnectionString,
                    SourceDatabaseType = EtlMapper.MapToVelocipedeDatabaseType(item.SourceDatabaseType),
                    TargetDatabaseType = EtlMapper.MapToVelocipedeDatabaseType(item.TargetDatabaseType),
                    Status = item.Status,
                    Time = item.Time.ToDateTime() // Converting from google.protobuf.Timestamp
                }).ToList()
            };
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching history for user {UserUid}", userUid);
            return StatusCode(503, ex.Message);
        }
    }
}
