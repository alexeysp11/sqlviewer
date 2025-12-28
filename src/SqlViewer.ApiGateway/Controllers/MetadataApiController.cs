using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Dtos.Metadata;
using SqlViewer.Common.Enums;

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
}
