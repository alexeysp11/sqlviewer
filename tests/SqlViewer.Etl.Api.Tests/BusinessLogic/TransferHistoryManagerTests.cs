using FluentAssertions;
using Moq;
using SqlViewer.Etl.Api.BusinessLogic;
using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Etl.Api.Tests.BusinessLogic;

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
                Source = "SourceDB",
                Target = "TargetDB",
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
        dto.Source.Should().Be(entities[0].Source);
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
            new() { CorrelationId = Guid.NewGuid(), Source = "S-01", Target = "T-01", CurrentStatus = TransferStatus.Failed },
            new() { CorrelationId = Guid.NewGuid(), Source = "S-02", Target = "T-02", CurrentStatus = TransferStatus.Started },
            new() { CorrelationId = Guid.NewGuid(), Source = "S-03", Target = "T-03", CurrentStatus = TransferStatus.Progress },
            new() { CorrelationId = lastGuid, Source = "S-04", Target = "T-04", CurrentStatus = TransferStatus.Completed }
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
