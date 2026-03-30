using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Enums;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Messages.Etl.Events;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Hosting;

/// <summary>
/// Monitors active sagas and marks them as TimedOut if no updates are received within the threshold.
/// </summary>
public sealed class SagaTimeoutWorker(IServiceScopeFactory scopeFactory, ILogger<SagaTimeoutWorker> logger) : BackgroundService
{
    private readonly TimeSpan _timeoutThreshold = TimeSpan.FromMinutes(60);
    private readonly TimeSpan _delay = TimeSpan.FromMinutes(1);
    private const int BatchSize = 25;

    private string ErrorMessageTimeout
        => $"Data tranfser timed out after {_timeoutThreshold.TotalMinutes} minutes of inactivity.";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessStalledMessages(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing Inbox messages.");
            }
            await Task.Delay(_delay, stoppingToken);
        }
    }

    public async Task ProcessStalledMessages(CancellationToken stoppingToken)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        DateTime deadline = DateTime.UtcNow - _timeoutThreshold;

        // Find sagas that are stuck in intermediate states
        List<DataTransferSagaEntity> stalledSagas = await db.DataTransferSagas
            .Where(s => s.CurrentState != TransferSagaStatus.Completed &&
                        s.CurrentState != TransferSagaStatus.Failed &&
                        s.CurrentState != TransferSagaStatus.TimedOut &&
                        s.CurrentState != TransferSagaStatus.Cancelled &&
                        s.LastUpdatedAt < deadline)
            .Take(BatchSize)
            .ToListAsync(stoppingToken);

        // Update stalled sagas
        foreach (DataTransferSagaEntity saga in stalledSagas)
        {
            logger.LogWarning("Saga {Id} timed out. Threshold: {Threshold} min. Last update: {Time}",
                saga.CorrelationId, _timeoutThreshold.TotalMinutes, saga.LastUpdatedAt);

            saga.CurrentState = TransferSagaStatus.TimedOut;
            saga.LastUpdatedAt = DateTime.UtcNow;

            // Add audit record for the timeout event
            db.TransferExecutions.Add(new TransferExecutionEntity
            {
                CorrelationId = saga.CorrelationId,
                TableName = saga.TableName,
                Status = TransferExecutionStatus.TimedOut,
                Progress = 0,
                LastErrorMessage = ErrorMessageTimeout
            });

            // Outbox message for compensation
            db.OutboxMessages.Add(new OutboxMessageEntity
            {
                CorrelationId = saga.CorrelationId,
                MessageType = nameof(DataTransferStatusUpdated),
                Topic = configuration[ConfigurationKeys.Services.Kafka.Topics.DataTransferStatusUpdates]!,
                UserUid = Guid.TryParse(saga.UserUid, out Guid userUid) ? userUid : null,
                Payload = JsonSerializer.Serialize(new DataTransferStatusUpdated(
                    MessageId: Guid.NewGuid(),
                    CorrelationId: saga.CorrelationId,
                    TransferStatus: TransferStatus.TimedOut.ToString(),
                    InternalStatus: TransferSagaStatus.TimedOut.ToString(),
                    Timestamp: DateTime.UtcNow,
                    ErrorMessage: ErrorMessageTimeout)),
                CreatedAt = DateTime.UtcNow,
                Error = ErrorMessageTimeout
            });
        }

        logger.LogInformation($"{nameof(SagaTimeoutWorker)}: processed {{Count}} stalled sagas.", stalledSagas.Count);

        await db.SaveChangesAsync(stoppingToken);
    }
}
