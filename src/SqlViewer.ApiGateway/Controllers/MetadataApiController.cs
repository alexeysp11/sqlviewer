using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Common.Extensions;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class MetadataApiController(ILogger<MetadataApiController> logger, IMetadataService metadataService) : ControllerBase
{
    private readonly ILogger<MetadataApiController> _logger = logger;
    private readonly IMetadataService _metadataService = metadataService;

    [HttpPost]
    [Route(RestApiPaths.Metadata.Tables)]
    public async Task<ActionResult<MetadataTablesResponseDto>> GetTablesAsync([FromBody] MetadataRequestDto request)
    {
        try
        {
            List<string> result = await _metadataService.GetTablesAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString);
            return Ok(new MetadataTablesResponseDto { Tables = result });
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to get tables in {DatabaseType}: {Message}", request.DatabaseType, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Metadata.Columns)]
    public async Task<ActionResult<MetadataColumnsResponseDto>> GetColumnsAsync([FromBody] MetadataRequestDto request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.TableName))
            {
                return BadRequest("Table name should be specified");
            }

            List<VelocipedeNativeColumnInfo> result = await _metadataService.GetColumnsAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                tableName: request.TableName);
            return Ok(new MetadataColumnsResponseDto
            {
                TableName = request.TableName,
                DatabaseType = request.DatabaseType,
                Columns = result.GetColumnInfoDtos(),
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Unable to get table columns for '{TableName}' in {DatabaseType}: {Message}",
                request.TableName,
                request.DatabaseType,
                ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
