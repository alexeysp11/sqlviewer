using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.QueryBuilder;
using SqlViewer.Common.Dtos.SqlQueries;
using SqlViewer.Common.Enums;
using SqlViewer.Common.Extensions;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class QueryBuilderApiController(
    ILogger<QueryBuilderApiController> logger,
    IQueryBuilderService queryBuilderService) : ControllerBase
{
    private readonly ILogger<QueryBuilderApiController> _logger = logger;
    private readonly IQueryBuilderService _queryBuilderService = queryBuilderService;

    [HttpPost]
    [Route(RestApiPaths.QueryBuilder.CreateTable)]
    public async Task<QueryBuilderResponseDto> GetCreateTableQueryAsync([FromBody] CreateTableRequestDto request)
    {
        try
        {
            string query = await _queryBuilderService.GetCreateTableQueryAsync(
                databaseType: request.DatabaseType,
                tableName: request.TableName,
                columnInfos: request.Columns.GetVelocipedeColumnInfos(request.DatabaseType));
            return new() { Status = SqlOperationStatus.Success, Query = query, };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }
}
