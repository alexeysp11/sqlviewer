using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Enums;
using StackExchange.Redis;

namespace SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;

public class DataTransferStep(
    ILogger<DataTransferStep> logger,
    IServiceScopeFactory scopeFactory,
    IConnectionMultiplexer redis) : ISagaStep
{
    public virtual async Task ExecuteAsync(DataTransferSagaEntity saga, CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();
        IDatabase redisDb = redis.GetDatabase();

        await LogStateAsync(db, saga, TransferExecutionStatus.Transferring);
        logger.LogInformation("[Saga {Id}] Starting data transfer loop...", saga.CorrelationId);
        try
        {
            int totalParts = 10;
            for (int i = 1; i <= totalParts; i++)
            {
                ct.ThrowIfCancellationRequested();
                await Task.Delay(500, ct);

                int progress = i * 10;

                // Updating Redis.
                await redisDb.StringSetAsync(
                    key: $"transfer:progress:{saga.CorrelationId}",
                    value: progress.ToString(),
                    expiry: TimeSpan.FromHours(1));
            }
            await LogStateAsync(db, saga, TransferExecutionStatus.Completed, 100);
        }
        catch (OperationCanceledException)
        {
            await LogStateAsync(db, saga, TransferExecutionStatus.Cancelled);
            logger.LogError("[Saga {Id}] Step cancelled", saga.CorrelationId);
            throw;
        }
        catch (Exception ex)
        {
            await LogStateAsync(db, saga, TransferExecutionStatus.Failed, error: ex.Message);
            logger.LogError(ex, "[Saga {Id}] Step failed", saga.CorrelationId);
            throw;
        }
    }

    private static async Task LogStateAsync(
        DataTransferDbContext db,
        DataTransferSagaEntity saga,
        TransferExecutionStatus status,
        double progress = 0,
        string? error = null)
    {
        db.TransferExecutions.Add(new TransferExecutionEntity
        {
            CorrelationId = saga.CorrelationId,
            TableName = saga.TableName,
            Status = status,
            Progress = progress,
            LastErrorMessage = error
        });

        // Use CancellationToken.None for final statuses
        await db.SaveChangesAsync(CancellationToken.None);
    }
}
