namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

public class SchemaValidationStep(ILogger<SchemaValidationStep> logger) : ISagaStep
{
    public virtual async Task ExecuteAsync(Guid correlationId, CancellationToken ct)
    {
        logger.LogInformation("[Saga {Id}] Step: Validating schemas and creating target table...", correlationId);

        // Stub: Simulating 'CREATE TABLE IF NOT EXISTS' logic
        await Task.Delay(800, ct);

        logger.LogInformation("[Saga {Id}] Step: Target schema is ready.", correlationId);
    }
}
