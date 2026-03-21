using SqlViewer.ApiGateway.IntegrationTests.Infrastructure.WebApplicationFactories;
using SqlViewer.Shared.Dtos.SqlQueries;
using System.Net.Http.Json;
using System.Net;
using SqlViewer.Shared.Constants;
using FluentAssertions;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.Enums;
using SqlViewer.Shared.Dtos.QueryBuilder;

namespace SqlViewer.ApiGateway.IntegrationTests.Controllers;

public sealed class QueryBuilderTests(ApiGatewayWebApplicationFactory<Program> factory)
    : IClassFixture<ApiGatewayWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateTable_ReturnsQuery_WhenAuthenticated()
    {
        await factory.EnsureDatabaseMigratedAsync();

        // Arrange
        CreateTableRequestDto request = new()
        {
            ConnectionString = factory.ConnectionString,
            DatabaseType = VelocipedeDatabaseType.PostgreSQL,
            TableName = "TestUsers",
            Columns =
            [
                new()
                {
                    ColumnName = "Id",
                    ColumnType = DbType.Int32,
                    IsPrimaryKey = true,
                    IsNullable = false
                },
                new()
                {
                    ColumnName = "Username",
                    ColumnType = DbType.String,
                    CharMaxLength = 255,
                    IsPrimaryKey = false,
                    IsNullable = false,
                    DefaultValue = "anonymous"
                },
                new()
                {
                    ColumnName = "Rating",
                    ColumnType = DbType.Decimal,
                    NumericPrecision = 18,
                    NumericScale = 2,
                    IsPrimaryKey = false,
                    IsNullable = true
                }
            ]
        };

        // Act.
        HttpResponseMessage response = await _client.PostAsJsonAsync(RestApiPaths.QueryBuilder.CreateTable, request);

        // Assert.
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        QueryBuilderResponseDto? result = await response.Content.ReadFromJsonAsync<QueryBuilderResponseDto>();
        result
            .Should()
            .NotBeNull();
        result.Query
            .Should()
            .Contain(@"CREATE TABLE ""TestUsers""")
            .And
            .Contain("Username");
    }

    [Fact]
    public async Task GetCreateTableQuery_EmptyTableName_ReturnsInternalServerError()
    {
        // Arrange
        CreateTableRequestDto request = new()
        {
            ConnectionString = factory.ConnectionString,
            DatabaseType = VelocipedeDatabaseType.PostgreSQL,
            TableName = string.Empty,
            Columns = []
        };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(RestApiPaths.QueryBuilder.CreateTable, request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }
}
