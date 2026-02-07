using FluentAssertions;
using Moq;
using SqlViewer.ApiGateway.Factories;
using SqlViewer.ApiGateway.Tests.Infrastructure.Models;
using SqlViewer.ApiGateway.VerticalSlices.QueryExecution.Services.Implementations;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Tests.Services.SqlQueryServiceTests;

public sealed class QueryTests
{
    private readonly IDbConnectionFactory _connectionFactory;

    public QueryTests()
    {
        IVelocipedeDbConnection dbConnection = GetDbConnectionMock();
        _connectionFactory = GetDbConnectionFactory(dbConnection);
    }

    #region Test cases
#pragma warning disable CA1024 // Use properties where appropriate
    public static TheoryData<TestCaseQuerying> GetTestCasesQuerying()
#pragma warning restore CA1024 // Use properties where appropriate
    {
        static TestCaseQuerying[] GetTestCases(string connectionString, string query) =>
        [
            new() { DatabaseType = VelocipedeDatabaseType.None, ConnectionString = connectionString, Query = query },
            new() { DatabaseType = VelocipedeDatabaseType.InMemory, ConnectionString = connectionString, Query = query },
            new() { DatabaseType = VelocipedeDatabaseType.SQLite, ConnectionString = connectionString, Query = query },
            new() { DatabaseType = VelocipedeDatabaseType.PostgreSQL, ConnectionString = connectionString, Query = query },
            new() { DatabaseType = VelocipedeDatabaseType.MSSQL, ConnectionString = connectionString, Query = query },
        ];

        TheoryData<TestCaseQuerying> result = [];
#nullable disable
        result.AddRange(GetTestCases(connectionString: null, query: null));
        result.AddRange(GetTestCases(connectionString: null, query: ""));
        result.AddRange(GetTestCases(connectionString: "", query: null));
        result.AddRange(GetTestCases(connectionString: "", query: ""));
        result.AddRange(GetTestCases(connectionString: "", query: "AnyQueryString"));
        result.AddRange(GetTestCases(connectionString: "AnyConnectionString", query: ""));
#nullable restore
        return result;
    }

    private static List<dynamic> QueryResult =>
    [
        new SimpleTableRecord { Id = 1, Name = "Name1", AdditionalInfo = null },
        new SimpleTableRecord { Id = 2, Name = "Name2", AdditionalInfo = "AdditionalInfo2" },
        new SimpleTableRecord { Id = 3, Name = "Name3", AdditionalInfo = null },
        new SimpleTableRecord { Id = 4, Name = "Name4", AdditionalInfo = null },
        new SimpleTableRecord { Id = 5, Name = "Name5", AdditionalInfo = "AdditionalInfo5" },
        new SimpleTableRecord { Id = 6, Name = "Name6", AdditionalInfo = "AdditionalInfo6" },
        new SimpleTableRecord { Id = 7, Name = "Name7", AdditionalInfo = "AdditionalInfo7" },
        new SimpleTableRecord { Id = 8, Name = "Name8", AdditionalInfo = null },
        new SimpleTableRecord { Id = 9, Name = "Name9", AdditionalInfo = null },
        new SimpleTableRecord { Id = 10, Name = "Name10", AdditionalInfo = "AdditionalInfo10" },
        new SimpleTableRecord { Id = 11, Name = "Name11", AdditionalInfo = "AdditionalInfo11" },
    ];

    private static IVelocipedeDbConnection GetDbConnectionMock()
    {
        Mock<IVelocipedeDbConnection> mockDbConnection = new();
        mockDbConnection
            .Setup(x => x.QueryAsync<dynamic>(It.IsAny<string>()))
            .ReturnsAsync(QueryResult);
        return mockDbConnection.Object;
    }

    private static IDbConnectionFactory GetDbConnectionFactory(IVelocipedeDbConnection dbConnection)
    {
        Mock<IDbConnectionFactory> factoryMock = new();
        factoryMock
            .Setup(x => x.GetDbConnectionAsync(It.IsAny<VelocipedeDatabaseType>(), It.IsAny<string>()))
            .ReturnsAsync(dbConnection);
        return factoryMock.Object;
    }
    #endregion  // Test cases

    [Theory]
    [MemberData(nameof(GetTestCasesQuerying))]
    public async Task QueryAsync(TestCaseQuerying testCase)
    {
        // Arrange.
        SqlQueryService sut = new(_connectionFactory);

        // Act.
        List<dynamic> result = await sut.QueryAsync(
            databaseType: testCase.DatabaseType,
            connectionString: testCase.ConnectionString,
            query: testCase.Query);

        // Assert.
        result
            .Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(QueryResult);
    }
}
