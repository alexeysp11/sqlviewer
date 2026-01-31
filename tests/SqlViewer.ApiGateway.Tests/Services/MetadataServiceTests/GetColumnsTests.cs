using FluentAssertions;
using Moq;
using SqlViewer.ApiGateway.Factories;
using SqlViewer.ApiGateway.Services.Implementations;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace SqlViewer.ApiGateway.Tests.Services.MetadataServiceTests;

public sealed class GetColumnsTests
{
    #region Test cases
#pragma warning disable CA1024 // Use properties where appropriate
    public static TheoryData<TestCaseGetMetadata> GetTestCasesMetadataColumns()
#pragma warning restore CA1024 // Use properties where appropriate
    {
        static TestCaseGetMetadata[] GetTestCases(string connectionString, string tableName) =>
        [
            new() { DatabaseType = VelocipedeDatabaseType.SQLite, ConnectionString = connectionString, TableName = tableName },
            new() { DatabaseType = VelocipedeDatabaseType.PostgreSQL, ConnectionString = connectionString, TableName = tableName },
            new() { DatabaseType = VelocipedeDatabaseType.MSSQL, ConnectionString = connectionString, TableName = tableName },
        ];

        TheoryData<TestCaseGetMetadata> result = [];
#nullable disable
        result.AddRange(GetTestCases(connectionString: null, tableName: null));
        result.AddRange(GetTestCases(connectionString: null, tableName: ""));
        result.AddRange(GetTestCases(connectionString: null, tableName: "AnyTableName"));
        
        result.AddRange(GetTestCases(connectionString: "", tableName: ""));
        result.AddRange(GetTestCases(connectionString: "", tableName: null));
        result.AddRange(GetTestCases(connectionString: "", tableName: "AnyTableName"));

        result.AddRange(GetTestCases(connectionString: "AnyConnectionString", tableName: null));
        result.AddRange(GetTestCases(connectionString: "AnyConnectionString", tableName: ""));
        result.AddRange(GetTestCases(connectionString: "AnyConnectionString", tableName: "AnyTableName"));
#nullable restore
        return result;
    }

    private static List<VelocipedeNativeColumnInfo> GetTablesResult(VelocipedeDatabaseType databaseType) =>
    [
        new()
        {
            DatabaseType = databaseType,
            ColumnName = "Id",
            NativeColumnType = "bigint",
            CharMaxLength = null,
            NumericPrecision = 64,
            NumericScale = 0,
            DefaultValue = null,
            IsPrimaryKey = false,
            IsNullable = false
        },
        new()
        {
            DatabaseType = databaseType,
            ColumnName = "Column1",
            NativeColumnType = "text",
            CharMaxLength = 254,
            NumericPrecision = null,
            NumericScale = null,
            DefaultValue = "DefaultValue",
            IsPrimaryKey = false,
            IsNullable = false
        },
        new()
        {
            DatabaseType = databaseType,
            ColumnName = "Column2",
            NativeColumnType = "text",
            CharMaxLength = 100,
            NumericPrecision = null,
            NumericScale = null,
            DefaultValue = null,
            IsPrimaryKey = false,
            IsNullable = true
        },
    ];

    private static IVelocipedeDbConnection GetDbConnectionMock(VelocipedeDatabaseType databaseType)
    {
        Mock<IVelocipedeDbConnection> mockDbConnection = new();
        mockDbConnection
            .Setup(x => x.DatabaseType)
            .Returns(databaseType);
        mockDbConnection
            .Setup(x => x.GetColumnsAsync(It.IsAny<string>()))
            .ReturnsAsync(GetTablesResult(databaseType));
        return mockDbConnection.Object;
    }

    private static IDbConnectionFactory GetDbConnectionFactory(IVelocipedeDbConnection dbConnection)
    {
        Mock<IDbConnectionFactory> factoryMock = new();
        factoryMock
            .Setup(x => x.GetDbConnectionAsync(dbConnection.DatabaseType, It.IsAny<string>()))
            .ReturnsAsync(dbConnection);
        return factoryMock.Object;
    }
    #endregion  // Test cases

    [Theory]
    [MemberData(nameof(GetTestCasesMetadataColumns))]
    public async Task GetColumnsAsync(TestCaseGetMetadata testCase)
    {
        // Arrange.
        IVelocipedeDbConnection dbConnection = GetDbConnectionMock(testCase.DatabaseType);
        IDbConnectionFactory connectionFactory = GetDbConnectionFactory(dbConnection);
        MetadataService sut = new(connectionFactory);

        // Act.
#nullable disable
        List<VelocipedeNativeColumnInfo> result = await sut.GetColumnsAsync(
            databaseType: testCase.DatabaseType,
            connectionString: testCase.ConnectionString,
            tableName: testCase.TableName);
#nullable restore

        // Assert.
        result
            .Should()
            .NotBeNullOrEmpty()
            .And
            .BeEquivalentTo(GetTablesResult(testCase.DatabaseType));
    }
}
