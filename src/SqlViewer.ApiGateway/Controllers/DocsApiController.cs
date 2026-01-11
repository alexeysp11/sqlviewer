using Microsoft.AspNetCore.Mvc;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos.Docs;
using SqlViewer.Common.Enums;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class DocsApiController(ILogger<DocsApiController> logger) : ControllerBase
{
    private readonly ILogger<DocsApiController> _logger = logger;

    [HttpGet]
    [Route(RestApiPaths.Docs.DbProviders)]
    public SqlViewerDocsResponseDto GetDbProviderDocs([FromQuery] VelocipedeDatabaseType databaseType)
    {
        try
        {
            string url = databaseType switch
            {
                VelocipedeDatabaseType.SQLite => "https://www.sqlite.org/index.html",
                VelocipedeDatabaseType.PostgreSQL => "https://www.postgresql.org/",
                VelocipedeDatabaseType.MSSQL => "https://learn.microsoft.com/en-us/sql/sql-server",
                VelocipedeDatabaseType.MySQL => "https://dev.mysql.com/doc/",
                _ => throw new NotImplementedException()
            };
            return new() { Status = SqlOperationStatus.Success, Url = url };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }
}
