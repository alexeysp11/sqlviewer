using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Sagas.SagaSteps;

public sealed class DataTransferStepTests
{
    private readonly Mock<ILogger<DataTransferStep>> _loggerMock = new();
    private readonly DataTransferStep _step;

    public DataTransferStepTests() => _step = new DataTransferStep(_loggerMock.Object);

    [Fact]
    public async Task ExecuteAsync_ShouldLogTransferStartAndFinish()
    {
        Guid correlationId = Guid.NewGuid();

        await _step.ExecuteAsync(correlationId, CancellationToken.None);

        _loggerMock.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Starting data transfer")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
}
