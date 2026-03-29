using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Enums;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;
using StackExchange.Redis;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Sagas.SagaSteps;

public sealed class DataTransferStepTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<ILogger<DataTransferStep>> _loggerMock = new();
    private readonly Mock<IConnectionMultiplexer> _redisMock = new();
    private readonly Mock<IDatabase> _dbMock = new();
    private readonly DataTransferStep _step;

    public DataTransferStepTests()
    {
        _redisMock
            .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_dbMock.Object);

        // In-Memory database.
        DbContextOptions<DataTransferDbContext> options = new DbContextOptionsBuilder<DataTransferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        DataTransferDbContext db = new(options);

        // DI Scope.
        Mock<IServiceScope> scopeMock = new();
        Mock<IServiceProvider> serviceProviderMock = new();

        _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
        scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(DataTransferDbContext))).Returns(db);

        _step = new DataTransferStep(_loggerMock.Object, _scopeFactoryMock.Object, _redisMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCompleteSuccessfully_And_CreateAuditLogs()
    {
        // Arrange
        Guid correlationId = _fixture.Create<Guid>();
        string tableName = _fixture.Create<string>();
        DataTransferSagaEntity transferSaga = _fixture.Build<DataTransferSagaEntity>()
            .With(x => x.CorrelationId, correlationId)
            .With(x => x.TableName, tableName)
            .Create();

        // We need access to the same context instance that Step uses.
        using IServiceScope scope = _scopeFactoryMock.Object.CreateScope();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        // Act
        await _step.ExecuteAsync(transferSaga, CancellationToken.None);

        // Assert
        // 1. Check Redis.
        _dbMock.Verify(x => x.StringSetAsync(
            $"transfer:progress:{correlationId}",
            "100",
            It.IsAny<TimeSpan?>(),
            false,
            When.Always,
            CommandFlags.None), Times.Once);

        // 2. Checking the audit in the database
        List<TransferExecutionEntity> auditLogs = db.TransferExecutions
            .Where(x => x.CorrelationId == correlationId)
            .OrderBy(x => x.Id)
            .ToList();
        List<TransferExecutionEntity> expectedLogs =
        [
            new TransferExecutionEntity
            {
                Status = TransferExecutionStatus.Transferring,
                Progress = 0,
                TableName = tableName,
                CorrelationId = correlationId
            },
            new TransferExecutionEntity
            {
                Status = TransferExecutionStatus.Completed,
                Progress = 100,
                TableName = tableName,
                CorrelationId = correlationId
            }
        ];
        auditLogs.Should().BeEquivalentTo(expectedLogs, options => options
            .Excluding(x => x.Id)
            .Excluding(x => x.Timestamp));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogCancelledStatus_WhenCancellationTokenIsSignalled()
    {
        // Arrange
        DataTransferSagaEntity transferSaga = _fixture.Create<DataTransferSagaEntity>();

        // Create a cancelled token.
        CancellationTokenSource cts = new();
        cts.Cancel();

        // We need access to the same context instance that Step uses.
        using IServiceScope scope = _scopeFactoryMock.Object.CreateScope();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        // Act
        Func<Task> act = async () => await _step.ExecuteAsync(transferSaga, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();

        TransferExecutionEntity? cancelledLog = db.TransferExecutions
            .FirstOrDefault(x => x.CorrelationId == transferSaga.CorrelationId && x.Status == TransferExecutionStatus.Cancelled);
        cancelledLog.Should().NotBeNull();
        cancelledLog!.Status.Should().Be(TransferExecutionStatus.Cancelled);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogFailure_WhenExceptionOccurs()
    {
        // Arrange
        DataTransferSagaEntity transferSaga = _fixture.Create<DataTransferSagaEntity>();

        // Simulate an error when working with Redis (or any other part of the cycle)
        _dbMock.Setup(x => x.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
               .ThrowsAsync(new Exception("Redis failure"));

        using IServiceScope scope = _scopeFactoryMock.Object.CreateScope();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        // Act
        Func<Task> act = async () => await _step.ExecuteAsync(transferSaga, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();

        TransferExecutionEntity? failureLog = db.TransferExecutions
            .FirstOrDefault(x => x.CorrelationId == transferSaga.CorrelationId && x.Status == TransferExecutionStatus.Failed);

        failureLog.Should().NotBeNull();
        failureLog!.LastErrorMessage.Should().Be("Redis failure");
    }
}
