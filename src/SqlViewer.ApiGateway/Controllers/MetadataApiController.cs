using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Enums;
using SqlViewer.ApiGateway.ViewModels.Metadata;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Factories;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class MetadataApiController(ILogger<MetadataApiController> logger) : ControllerBase
{
    private readonly ILogger<MetadataApiController> _logger = logger;

    [HttpPost]
    [Route("/api/metadata/tables")]
    public async Task<MetadataTablesResponse> GetTablesAsync([FromBody] MetadataRequest request)
    {
        try
        {
            using IVelocipedeDbConnection dbConnection
                = VelocipedeDbConnectionFactory.InitializeDbConnection(request.DatabaseType, request.ConnectionString);
            List<string> result = await dbConnection.GetTablesInDbAsync();
            return new() { Status = SqlOperationStatus.Success, Tables = result, };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }
}
