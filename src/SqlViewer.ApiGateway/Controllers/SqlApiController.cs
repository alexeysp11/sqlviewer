using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.SqlQueries;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class SqlApiController(ILogger<SqlApiController> logger, ISqlQueryService sqlQueryService) : ControllerBase
{
    private readonly ILogger<SqlApiController> _logger = logger;
    private readonly ISqlQueryService _sqlQueryService = sqlQueryService;

    [HttpPost]
    [Route(RestApiPaths.Sql.Query)]
    public async Task<ActionResult<SqlQueryResponseDto>> QueryAsync([FromBody] SqlQueryRequestDto request)
    {
        try
        {
            List<dynamic> result = await _sqlQueryService.QueryAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                query: request.Query);
            return Ok(new SqlQueryResponseDto { QueryResult = result });
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Sql.Execute)]
    public async Task<IActionResult> ExecuteAsync([FromBody] SqlQueryRequestDto request)
    {
        try
        {
            await _sqlQueryService.ExecuteAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                query: request.Query);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Sql.CreateTable)]
    public async Task<IActionResult> CreateTableAsync([FromBody] CreateTableRequestDto request)
    {
        try
        {
            await _sqlQueryService.CreateTableAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                tableName: request.TableName,
                columnInfos: request.GetVelocipedeColumnInfos());
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Sql.DropTable)]
    public async Task<IActionResult> DropTableAsync([FromBody] DropTableRequestDto request)
    {
        try
        {
            await _sqlQueryService.DropTableAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                tableName: request.TableName);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
