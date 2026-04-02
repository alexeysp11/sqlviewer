using AutoFixture;
using FluentAssertions;
using Moq;
using SqlViewer.Etl.Api.BusinessLogic.Implementations;
using SqlViewer.Etl.Api.Repositories.Abstractions;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Dtos.Etl;
using StackExchange.Redis;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Etl.Api.Tests.Unit.BusinessLogic.TransferHistoryManagerTests;

public sealed class GetHistoryTests
{
    private readonly Mock<ITransferHistoryRepository> _repositoryMock;
    private readonly TransferHistoryManager _manager;
    private readonly Fixture _autoFixture;

    public GetHistoryTests()
    {
        _repositoryMock = new Mock<ITransferHistoryRepository>();
        _manager = new TransferHistoryManager(_repositoryMock.Object, Mock.Of<IConnectionMultiplexer>());
        _autoFixture = new Fixture();
    }

    [Fact]
    public async Task GetHistoryAsync_ShouldMapEntitiesToDtosCorrectly()
    {
        // Arrange
        Guid userUid = Guid.NewGuid();
        List<TransferJobEntity> entities =
        [
            new()
            {
                CorrelationId = Guid.NewGuid(),
                UserUid = userUid,
                SourceConnectionString = _autoFixture.Create<string>(),
                TargetConnectionString = _autoFixture.Create<string>(),
                SourceDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TargetDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TableName = _autoFixture.Create<string>(),
                CurrentStatus = TransferStatus.Completed,
                CreatedAt = DateTime.UtcNow
            }
        ];

        _repositoryMock
            .Setup(r => r.GetHistoryAsync(userUid, null, 10))
            .ReturnsAsync(entities);

        // Act
        TransferHistoryResponseDto result = await _manager.GetHistoryAsync(userUid, null, 10);

        // Assert
        result.Items.Should().HaveCount(1);
        TransferJobDto dto = result.Items.First();
        dto.CorrelationId.Should().Be(entities[0].CorrelationId);
        dto.SourceConnectionString.Should().Be(entities[0].SourceConnectionString);
        dto.Status.Should().Be(TransferStatus.Completed.ToString());
        dto.Time.Should().Be(entities[0].CreatedAt);
    }

    [Fact]
    public async Task GetHistoryAsync_WhenListIsNotEmpty_ShouldReturnLastCorrelationIdAsCursor()
    {
        // Arrange
        Guid userUid = Guid.NewGuid();
        Guid lastGuid = Guid.NewGuid();
        List<TransferJobEntity> entities =
        [
            new()
            {
                CorrelationId = Guid.NewGuid(),
                UserUid = userUid,
                SourceConnectionString = _autoFixture.Create<string>(),
                TargetConnectionString = _autoFixture.Create<string>(),
                SourceDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TargetDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TableName = _autoFixture.Create<string>(),
                CurrentStatus = TransferStatus.Failed
            },
            new()
            {
                CorrelationId = Guid.NewGuid(),
                UserUid = userUid,
                SourceConnectionString = _autoFixture.Create<string>(),
                TargetConnectionString = _autoFixture.Create<string>(),
                SourceDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TargetDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TableName = _autoFixture.Create<string>(),
                CurrentStatus = TransferStatus.Started
            },
            new()
            {
                CorrelationId = Guid.NewGuid(),
                UserUid = userUid,
                SourceConnectionString = _autoFixture.Create<string>(),
                TargetConnectionString = _autoFixture.Create<string>(),
                SourceDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TargetDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TableName = _autoFixture.Create<string>(),
                CurrentStatus = TransferStatus.Progress
            },
            new()
            {
                CorrelationId = lastGuid,
                UserUid = userUid,
                SourceConnectionString = _autoFixture.Create<string>(),
                TargetConnectionString = _autoFixture.Create<string>(),
                SourceDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TargetDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TableName = _autoFixture.Create<string>(),
                CurrentStatus = TransferStatus.Completed
            }
        ];

        _repositoryMock
            .Setup(r => r.GetHistoryAsync(userUid, null, entities.Count))
            .ReturnsAsync(entities);

        // Act
        TransferHistoryResponseDto result = await _manager.GetHistoryAsync(userUid, null, entities.Count);

        // Assert
        result.CursorCorrelationId.Should().Be(lastGuid);
    }

    [Fact]
    public async Task GetHistoryAsync_WhenListIsEmpty_ShouldReturnNullCursor()
    {
        // Arrange
        Guid userUid = Guid.NewGuid();
        int limit = 10;
        _repositoryMock
            .Setup(r => r.GetHistoryAsync(userUid, null, limit))
            .ReturnsAsync([]);

        // Act
        TransferHistoryResponseDto result = await _manager.GetHistoryAsync(userUid, null, limit);

        // Assert
        result.Items.Should().BeEmpty();
        result.CursorCorrelationId.Should().BeNull();
    }
}
