using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Sagas.SagaSteps;

public sealed class AccessabilityCheckStepTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<ILogger<AccessabilityCheckStep>> _loggerMock = new();
    private readonly AccessabilityCheckStep _step;

    public AccessabilityCheckStepTests() => _step = new AccessabilityCheckStep(_loggerMock.Object);

    [Fact]
    public async Task ExecuteAsync_ShouldLogStepsAndComplete()
    {
        Guid correlationId = _fixture.Create<Guid>();
        DataTransferSagaEntity transferSaga = _fixture.Build<DataTransferSagaEntity>()
            .With(x => x.CorrelationId, correlationId)
            .Create();

        await _step.ExecuteAsync(transferSaga, CancellationToken.None);

        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Checking database accessibility")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);

        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("All databases are reachable")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
}
