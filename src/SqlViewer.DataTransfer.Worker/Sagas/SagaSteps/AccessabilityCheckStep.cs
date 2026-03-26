namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

public sealed class AccessabilityCheckStep(ILogger<AccessabilityCheckStep> logger) : ISagaStep
{
    public async Task ExecuteAsync(Guid correlationId, CancellationToken ct)
    {
        logger.LogInformation("[Saga {Id}] Step: Checking database accessibility...", correlationId);

        // Stub: Simulating DB ping/connection check
        await Task.Delay(500, ct);

        logger.LogInformation("[Saga {Id}] Step: All databases are reachable.", correlationId);
    }
}
