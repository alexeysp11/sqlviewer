using SqlViewer.DataTransfer.Worker.Data.Entities;

namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

public class AccessabilityCheckStep(ILogger<AccessabilityCheckStep> logger) : ISagaStep
{
    public virtual async Task ExecuteAsync(DataTransferSagaEntity saga, CancellationToken ct)
    {
        logger.LogInformation("[Saga {Id}] Step: Checking database accessibility...", saga.CorrelationId);

        // Stub: Simulating DB ping/connection check
        await Task.Delay(500, ct);

        logger.LogInformation("[Saga {Id}] Step: All databases are reachable.", saga.CorrelationId);
    }
}
