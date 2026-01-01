using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Common.Enums;
using SqlViewer.Common.Extensions;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class MetadataApiController(ILogger<MetadataApiController> logger, IMetadataService metadataService) : ControllerBase
{
    private readonly ILogger<MetadataApiController> _logger = logger;
    private readonly IMetadataService _metadataService = metadataService;

    [HttpPost]
    [Route("/api/metadata/tables")]
    public async Task<MetadataTablesResponseDto> GetTablesAsync([FromBody] MetadataRequestDto request)
    {
        try
        {
            List<string> result = await _metadataService.GetTablesAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString);
            return new() { Status = SqlOperationStatus.Success, Tables = result, };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }

    [HttpPost]
    [Route("/api/metadata/columns")]
    public async Task<MetadataColumnsResponseDto> GetColumnsAsync([FromBody] MetadataRequestDto request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.TableName))
            {
                throw new InvalidOperationException("Table name should be specified");
            }

            List<VelocipedeNativeColumnInfo> result = await _metadataService.GetColumnsAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                tableName: request.TableName);
            return new()
            {
                Status = SqlOperationStatus.Success,
                TableName = request.TableName,
                DatabaseType = request.DatabaseType,
                Columns = result.GetColumnInfoDtos(),
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new()
            {
                Status = SqlOperationStatus.Failed,
                ErrorMessage = ex.Message,
                TableName = request.TableName,
                DatabaseType = request.DatabaseType,
            };
        }
    }
}
