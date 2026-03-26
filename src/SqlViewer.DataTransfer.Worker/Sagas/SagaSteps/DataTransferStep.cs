namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

public sealed class DataTransferStep(ILogger<DataTransferStep> logger) : ISagaStep
{
    public async Task ExecuteAsync(Guid correlationId, CancellationToken ct)
    {
        logger.LogInformation("[Saga {Id}] Step: Starting data transfer (Simulated)...", correlationId);

        // Stub: The main 'Task.Delay' representing long-running operation
        // In real scenario, here we'll have a loop updating Redis progress
        await Task.Delay(5000, ct);

        logger.LogInformation("[Saga {Id}] Step: Data transfer completed successfully.", correlationId);
    }
}
