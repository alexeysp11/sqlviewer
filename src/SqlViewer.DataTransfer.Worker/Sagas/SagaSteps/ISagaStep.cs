namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

/// <summary>
/// Represents a single unit of work within the data transfer saga.
/// </summary>
public interface ISagaStep
{
    /// <summary>
    /// Executes the specific logic for this step.
    /// </summary>
    Task ExecuteAsync(Guid correlationId, CancellationToken ct);
}
