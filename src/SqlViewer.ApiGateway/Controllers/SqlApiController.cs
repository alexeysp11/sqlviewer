using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Common.Enums;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class SqlApiController(ILogger<SqlApiController> logger, ISqlQueryService sqlQueryService) : ControllerBase
{
    private readonly ILogger<SqlApiController> _logger = logger;
    private readonly ISqlQueryService _sqlQueryService = sqlQueryService;

    [HttpPost]
    [Route(RestApiPaths.Sql.Query)]
    public async Task<SqlQueryResponseDto> QueryAsync([FromBody] SqlQueryRequestDto request)
    {
        try
        {
            List<dynamic> result = await _sqlQueryService.QueryAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                query: request.Query);
            return new() { Status = SqlOperationStatus.Success, QueryResult = result, };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Sql.Execute)]
    public async Task<CommonResponseDto> ExecuteAsync([FromBody] SqlQueryRequestDto request)
    {
        try
        {
            await _sqlQueryService.ExecuteAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                query: request.Query);
            return new() { Status = SqlOperationStatus.Success, };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Sql.CreateTable)]
    public async Task<CommonResponseDto> CreateTableAsync([FromBody] CreateTableRequestDto request)
    {
        try
        {
            await _sqlQueryService.CreateTableAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                tableName: request.TableName,
                columnInfos: request.GetVelocipedeColumnInfos());
            return new() { Status = SqlOperationStatus.Success, };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }

    [HttpPost]
    [Route(RestApiPaths.Sql.DropTable)]
    public async Task<CommonResponseDto> DropTableAsync([FromBody] DropTableRequestDto request)
    {
        try
        {
            await _sqlQueryService.DropTableAsync(
                databaseType: request.DatabaseType,
                connectionString: request.ConnectionString,
                tableName: request.TableName);
            return new() { Status = SqlOperationStatus.Success, };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }
}
