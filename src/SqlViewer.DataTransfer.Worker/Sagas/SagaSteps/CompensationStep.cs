namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

public class CompensationStep(ILogger<CompensationStep> logger) : ISagaStep
{
    public virtual async Task ExecuteAsync(Guid correlationId, CancellationToken ct)
    {
        logger.LogWarning("[Saga {Id}] Step: Executing compensation logic (Cleanup)...", correlationId);

        // Stub: Simulating 'DROP TABLE IF EXISTS' in target DB
        await Task.Delay(300, ct);

        logger.LogWarning("[Saga {Id}] Step: Cleanup finished.", correlationId);
    }
}
