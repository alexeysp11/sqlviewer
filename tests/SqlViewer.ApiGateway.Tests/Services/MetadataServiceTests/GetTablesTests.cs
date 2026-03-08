using FluentAssertions;
using Moq;
using SqlViewer.ApiGateway.VerticalSlices.Metadata.Services.Implementations;
using SqlViewer.Common.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.ApiGateway.Tests.Services.MetadataServiceTests;

public sealed class GetTablesTests
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetTablesTests()
    {
        IVelocipedeDbConnection dbConnection = GetDbConnectionMock();
        _connectionFactory = GetDbConnectionFactory(dbConnection);
    }

    #region Test case
#pragma warning disable CA1024 // Use properties where appropriate
    public static TheoryData<TestCaseGetMetadata> GetTestCasesMetadataTables()
#pragma warning restore CA1024 // Use properties where appropriate
    {
        static TestCaseGetMetadata[] GetTestCases(string connectionString) =>
        [
            new() { DatabaseType = VelocipedeDatabaseType.None, ConnectionString = connectionString },
            new() { DatabaseType = VelocipedeDatabaseType.InMemory, ConnectionString = connectionString },
            new() { DatabaseType = VelocipedeDatabaseType.SQLite, ConnectionString = connectionString },
            new() { DatabaseType = VelocipedeDatabaseType.PostgreSQL, ConnectionString = connectionString },
            new() { DatabaseType = VelocipedeDatabaseType.MSSQL, ConnectionString = connectionString },
        ];

        TheoryData<TestCaseGetMetadata> result = [];
#nullable disable
        result.AddRange(GetTestCases(connectionString: null));
        result.AddRange(GetTestCases(connectionString: ""));
        result.AddRange(GetTestCases(connectionString: "AnyConnectionString"));
#nullable restore
        return result;
    }

    private static List<string> TablesResult => ["Table1", "Table2", "Table3", "Table4", "Table5"];

    private static IVelocipedeDbConnection GetDbConnectionMock()
    {
        Mock<IVelocipedeDbConnection> mockDbConnection = new();
        mockDbConnection
            .Setup(x => x.GetTablesInDbAsync())
            .ReturnsAsync(TablesResult);
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
    #endregion  // Test case

    [Theory]
    [MemberData(nameof(GetTestCasesMetadataTables))]
    public async Task GetTablesAsync(TestCaseGetMetadata testCase)
    {
        // Arrange.
        MetadataService sut = new(_connectionFactory);

        // Act.
        List<string> result = await sut.GetTablesAsync(
            databaseType: testCase.DatabaseType,
            connectionString: testCase.ConnectionString);

        // Assert.
        result
            .Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(TablesResult);
    }
}
