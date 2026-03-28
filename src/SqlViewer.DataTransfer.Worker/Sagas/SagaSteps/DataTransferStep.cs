using StackExchange.Redis;

namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

public class DataTransferStep(
    ILogger<DataTransferStep> logger,
    IConnectionMultiplexer redis) : ISagaStep
{
    public virtual async Task ExecuteAsync(Guid correlationId, CancellationToken ct)
    {
        IDatabase redisDb = redis.GetDatabase();

        logger.LogInformation("[Saga {Id}] Starting data transfer loop...", correlationId);

        int totalParts = 10;
        for (int i = 1; i <= totalParts; i++)
        {
            ct.ThrowIfCancellationRequested();

            await Task.Delay(500, ct);

            int progress = i * 10;

            await redisDb.StringSetAsync(
                $"transfer:progress:{correlationId}",
                progress.ToString(),
                TimeSpan.FromHours(1));

            logger.LogDebug("[Saga {Id}] Progress: {Percent}%", correlationId, progress);
        }
    }
}
