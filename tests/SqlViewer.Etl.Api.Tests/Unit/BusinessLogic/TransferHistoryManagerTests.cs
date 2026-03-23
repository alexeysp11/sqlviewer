using FluentAssertions;
using Moq;
using SqlViewer.Etl.Api.BusinessLogic;
using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Dtos.Etl;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Etl.Api.Tests.Unit.BusinessLogic;

public sealed class TransferHistoryManagerTests
{
    private readonly Mock<ITransferHistoryRepository> _repositoryMock;
    private readonly TransferHistoryManager _manager;

    public TransferHistoryManagerTests()
    {
        _repositoryMock = new Mock<ITransferHistoryRepository>();
        _manager = new TransferHistoryManager(_repositoryMock.Object);
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
                SourceConnectionString = "SourceDB",
                TargetConnectionString = "TargetDB",
                SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
                TargetDatabaseType = VelocipedeDatabaseType.SQLite,
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
                SourceConnectionString = "S-01",
                TargetConnectionString = "T-01",
                SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
                TargetDatabaseType = VelocipedeDatabaseType.SQLite,
                CurrentStatus = TransferStatus.Failed
            },
            new()
            {
                CorrelationId = Guid.NewGuid(),
                UserUid = userUid,
                SourceConnectionString = "S-02",
                TargetConnectionString = "T-02",
                SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
                TargetDatabaseType = VelocipedeDatabaseType.SQLite,
                CurrentStatus = TransferStatus.Started
            },
            new()
            {
                CorrelationId = Guid.NewGuid(),
                UserUid = userUid,
                SourceConnectionString = "S-03",
                TargetConnectionString = "T-03",
                SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
                TargetDatabaseType = VelocipedeDatabaseType.SQLite,
                CurrentStatus = TransferStatus.Progress
            },
            new()
            {
                CorrelationId = lastGuid,
                UserUid = userUid,
                SourceConnectionString = "S-04",
                TargetConnectionString = "T-04",
                SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
                TargetDatabaseType = VelocipedeDatabaseType.SQLite,
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
