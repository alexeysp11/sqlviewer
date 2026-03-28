using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;
using StackExchange.Redis;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Sagas.SagaSteps;

public sealed class DataTransferStepTests
{
    private readonly Mock<ILogger<DataTransferStep>> _loggerMock = new();
    private readonly Mock<IConnectionMultiplexer> _redisMock = new();
    private readonly Mock<IDatabase> _dbMock = new();
    private readonly DataTransferStep _step;

    public DataTransferStepTests()
    {
        _redisMock
            .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_dbMock.Object);

        _step = new DataTransferStep(_loggerMock.Object, _redisMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCompleteSuccessfully_UpdatingRedisAndLogging()
    {
        // Arrange
        Guid correlationId = Guid.NewGuid();
        CancellationToken ct = CancellationToken.None;

        // Act
        await _step.ExecuteAsync(correlationId, ct);

        // Assert
        // We check the final result (100%)
        _dbMock.Verify(x => x.StringSetAsync(
            $"transfer:progress:{correlationId}",
            "100",
            It.Is<TimeSpan?>(ts => ts == TimeSpan.FromHours(1)),
            false,
            When.Always,
            CommandFlags.None),
        Times.Once);

        // Check the number of iterations (exactly 10 entries in Redis)
        _dbMock.Verify(x => x.StringSetAsync(
            It.Is<RedisKey>(k => k.ToString().Contains(correlationId.ToString())),
            It.IsAny<RedisValue>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<bool>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>()),
        Times.Exactly(10));

        // Checking logs
        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Starting data transfer")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
}
