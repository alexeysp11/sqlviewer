namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

public class AccessabilityCheckStep(ILogger<AccessabilityCheckStep> logger) : ISagaStep
{
    public virtual async Task ExecuteAsync(Guid correlationId, CancellationToken ct)
    {
        logger.LogInformation("[Saga {Id}] Step: Checking database accessibility...", correlationId);

        // Stub: Simulating DB ping/connection check
        await Task.Delay(500, ct);

        logger.LogInformation("[Saga {Id}] Step: All databases are reachable.", correlationId);
    }
}
