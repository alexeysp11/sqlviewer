using Microsoft.Extensions.Caching.Distributed;

namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

public class DataTransferStep(
    ILogger<DataTransferStep> logger,
    IDistributedCache redis) : ISagaStep
{
    public virtual async Task ExecuteAsync(Guid correlationId, CancellationToken ct)
    {
        logger.LogInformation("[Saga {Id}] Starting data transfer loop...", correlationId);

        int totalParts = 10;
        for (int i = 1; i <= totalParts; i++)
        {
            ct.ThrowIfCancellationRequested();

            await Task.Delay(500, ct);

            int progress = i * 10;

            await redis.SetStringAsync(
                $"transfer:progress:{correlationId}",
                progress.ToString(),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) },
                ct);

            logger.LogDebug("[Saga {Id}] Progress: {Percent}%", correlationId, progress);
        }
    }
}
