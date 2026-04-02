using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Sagas.SagaSteps;

public sealed class CompensationStepTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<ILogger<CompensationStep>> _loggerMock = new();
    private readonly CompensationStep _step;

    public CompensationStepTests() => _step = new CompensationStep(_loggerMock.Object);

    [Fact]
    public async Task ExecuteAsync_ShouldLogWarningDuringCleanup()
    {
        Guid correlationId = _fixture.Create<Guid>();
        DataTransferSagaEntity transferSaga = _fixture.Build<DataTransferSagaEntity>()
            .With(x => x.CorrelationId, correlationId)
            .Create();

        await _step.ExecuteAsync(transferSaga, CancellationToken.None);

        _loggerMock.Verify(x => x.Log(
            LogLevel.Warning, // Проверяем именно Warning
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Executing compensation logic")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);

        _loggerMock.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Cleanup finished")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
}
