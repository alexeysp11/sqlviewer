using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Enums;
using SqlViewer.ApiGateway.ViewModels.Docs;
using SqlViewer.ApiGateway.ViewModels.Metadata;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Factories;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class DocsApiController(ILogger<DocsApiController> logger) : ControllerBase
{
    private readonly ILogger<DocsApiController> _logger = logger;

    [HttpPost]
    [Route("/api/docs/db-providers")]
    public SqlViewerDocsResponse GetDbProviderDocs([FromQuery] VelocipedeDatabaseType databaseType)
    {
        try
        {
            string url = databaseType switch
            {
                VelocipedeDatabaseType.SQLite => "https://www.sqlite.org/index.html",
                VelocipedeDatabaseType.PostgreSQL => "https://www.postgresql.org/",
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
