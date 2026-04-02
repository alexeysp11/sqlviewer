using FluentAssertions;
using Moq;
using SqlViewer.Etl.Api.BusinessLogic.Implementations;
using SqlViewer.Etl.Api.Repositories.Abstractions;
using SqlViewer.Etl.Core.Data.Projections;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Dtos.Etl;
using StackExchange.Redis;

namespace SqlViewer.Etl.Api.Tests.Unit.BusinessLogic.TransferHistoryManagerTests;

public class GetStatusesTests
{
    private readonly Mock<ITransferHistoryRepository> _repositoryMock;
    private readonly Mock<IDatabase> _redisDbMock;
    private readonly TransferHistoryManager _sut; // System Under Test

    public GetStatusesTests()
    {
        _repositoryMock = new Mock<ITransferHistoryRepository>();
        _redisDbMock = new Mock<IDatabase>();

        Mock<IConnectionMultiplexer> redisMultiplexerMock = new();
        redisMultiplexerMock
            .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_redisDbMock.Object);

        _sut = new TransferHistoryManager(_repositoryMock.Object, redisMultiplexerMock.Object);
    }

    [Fact]
    public async Task GetStatusesAsync_ShouldCombineDbAndRedisData()
    {
        // Arrange
        Guid userUid = Guid.NewGuid();
        Guid correlationId = Guid.NewGuid();
        Guid[] ids = [correlationId];

        // Data from database.
        List<TransferJobDbProjection> dbProjections =
        [
            new(correlationId, TransferStatus.Started, false)
        ];

        _repositoryMock
            .Setup(x => x.GetStatusesAsync(userUid, ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbProjections);

        // Data from Redis (simulating MGET)
        // Dapper/Redis uses specific types, so we return RedisValue
        _redisDbMock
            .Setup(x => x.StringGetAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(["45.5"]);

        // Act
        BatchTransferStatusesResponseDto result = await _sut.GetStatusesAsync(userUid, ids);

        // Assert
        result.Items.Should().HaveCount(1);
        TransferStatusResponseDto item = result.Items.First();
        item.CorrelationId.Should().Be(correlationId);
        item.Progress.Should().Be(45.5);
        item.IsFinalState.Should().BeFalse();
        item.StatusMessage.Should().Be("Started");
    }

    [Fact]
    public async Task GetStatusesAsync_WhenRedisIsEmpty_ShouldReturnZeroProgress()
    {
        // Arrange
        Guid userUid = Guid.NewGuid();
        Guid correlationId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetStatusesAsync(userUid, It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new(correlationId, TransferStatus.Started, false)]);

        _redisDbMock
            .Setup(x => x.StringGetAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync([RedisValue.Null]);

        // Act
        BatchTransferStatusesResponseDto result = await _sut.GetStatusesAsync(userUid, [correlationId]);

        // Assert
        result.Items.First().Progress.Should().Be(0.0);
    }

    [Fact]
    public async Task GetStatusesAsync_WhenDbReturnsEmpty_ShouldReturnEmptyResponse()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetStatusesAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        BatchTransferStatusesResponseDto result = await _sut.GetStatusesAsync(Guid.NewGuid(), [Guid.NewGuid()]);

        // Assert
        result.Items.Should().BeEmpty();

        // Check that we didn't even access the data from Redis.
        _redisDbMock.Verify(x => x.StringGetAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()), Times.Never);
    }
}
