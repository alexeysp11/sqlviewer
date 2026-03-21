using System.Data;
using FluentAssertions;
using Moq;
using SqlViewer.ApiHandlers;
using SqlViewer.Shared.Dtos.SqlQueries;
using SqlViewer.Services.Implementations;
using SqlViewer.Tests.Infrastructure.Models;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace SqlViewer.Tests.Services.SqlApiServiceTests;

public sealed class QueryTests
{
    #region Test cases
    public static List<dynamic>? NullQueryResult;
    public static List<dynamic> EmptyQueryResult = [];
    public static List<dynamic> ValidQueryResult =
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

    public static TheoryData<SqlQueryResponseDto> TestCasesQuerying =>
    [
        new SqlQueryResponseDto { QueryResult = NullQueryResult },
        new SqlQueryResponseDto { QueryResult = EmptyQueryResult },
        new SqlQueryResponseDto { QueryResult = ValidQueryResult },
    ];
    #endregion  // Test cases

    [Fact]
    public async Task QueryAsync_ResponseDtoIsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        SqlQueryResponseDto? responseDto = null;
        Mock<ISqlHttpHandler> mockClient = new();
        mockClient
            .Setup(x => x.ExecuteQueryAsync(It.IsAny<SqlQueryRequestDto>()))
            .ReturnsAsync(responseDto);

        SqlApiService service = new(mockClient.Object);
        var act = async () => await service.QueryAsync(VelocipedeDatabaseType.PostgreSQL, "conn_string", "SELECT 1");

        // Act & Assert
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Unable to get response DTO");
    }

    [Fact]
    public async Task QueryAsync_StatusIsFailed_ThrowsInvalidOperationException()
    {
        // Arrange
        Mock<ISqlHttpHandler> mockClient = new();
        mockClient
            .Setup(x => x.ExecuteQueryAsync(It.IsAny<SqlQueryRequestDto>()))
            .ThrowsAsync(new Exception("Database error"));

        SqlApiService service = new(mockClient.Object);
        var act = async () => await service.QueryAsync(VelocipedeDatabaseType.PostgreSQL, "conn_string", "SELECT 1");

        // Act & Assert
        await act
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage("Database error");
    }

    [Theory]
    [MemberData(nameof(TestCasesQuerying), Skip = "This test is unstable due to incorrect DataTable comparison")]
    public async Task QueryAsync_StatusIsSuccess(SqlQueryResponseDto responseDto)
    {
        // Arrange.
        Mock<ISqlHttpHandler> mockClient = new();
        mockClient
            .Setup(x => x.ExecuteQueryAsync(It.IsAny<SqlQueryRequestDto>()))
            .ReturnsAsync(responseDto);

        DataTable expected = responseDto.QueryResult?.ToDataTable() ?? new DataTable();

        SqlApiService service = new(mockClient.Object);

        // Act.
        DataTable result = await service.QueryAsync(VelocipedeDatabaseType.PostgreSQL, "conn_string", "SELECT 1");

        // Assert.
        result
            .Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expected);
    }
}
