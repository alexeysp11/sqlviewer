using SqlViewer.ApiGateway.IntegrationTests.Infrastructure.WebApplicationFactories;
using SqlViewer.Common.Dtos.SqlQueries;
using System.Net.Http.Json;
using System.Net;
using SqlViewer.Common.Constants;

namespace SqlViewer.ApiGateway.IntegrationTests.Controllers;

public class QueryBuilderTests(ApiGatewayWebApplicationFactory<Program> factory) : IClassFixture<ApiGatewayWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateTable_ReturnsQuery_WhenAuthenticated()
    {
        await factory.EnsureDatabaseMigratedAsync();

        //CreateTableRequestDto request = new CreateTableRequestDto { /* ... */ };
        CreateTableRequestDto? request = null;

        HttpResponseMessage response = await _client.PostAsJsonAsync(RestApiPaths.QueryBuilder.CreateTable, request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
