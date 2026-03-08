using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Docs;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Controllers.Docs;

[ApiController]
[Route("[controller]")]
public sealed class DocsApiController(ILogger<DocsApiController> logger) : ControllerBase
{
    private readonly ILogger<DocsApiController> _logger = logger;

    [HttpGet]
    [Authorize]
    [Route(RestApiPaths.Docs.DbProviders)]
    public ActionResult<SqlViewerDocsResponseDto> GetDbProviderDocs([FromQuery] VelocipedeDatabaseType databaseType)
    {
        try
        {
            string url = databaseType switch
            {
                VelocipedeDatabaseType.SQLite => "https://www.sqlite.org/index.html",
                VelocipedeDatabaseType.PostgreSQL => "https://www.postgresql.org/",
                VelocipedeDatabaseType.MSSQL => "https://learn.microsoft.com/en-us/sql/sql-server",
                VelocipedeDatabaseType.MySQL => "https://dev.mysql.com/doc/",
                _ => throw new NotSupportedException($"Unable to get documentation for the specified database type: {databaseType}")
            };
            return Ok(new SqlViewerDocsResponseDto { Url = url });
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
