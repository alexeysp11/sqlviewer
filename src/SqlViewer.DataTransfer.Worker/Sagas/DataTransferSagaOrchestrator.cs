using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Messages.Etl.Commands;
using SqlViewer.Shared.Messages.Etl.Events;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Sagas;

/// <summary>
/// Orchestrates state transitions based on incoming messages from the Inbox.
/// </summary>
public sealed class DataTransferSagaOrchestrator(
    IServiceScopeFactory scopeFactory,
    ILogger<DataTransferSagaOrchestrator> logger,
    AccessabilityCheckStep accessStep,
    SchemaValidationStep schemaStep,
    DataTransferStep transferStep,
    CompensationStep compensationStep) : IDataTransferSagaOrchestrator
{
    public async Task ExecuteAsync(Guid correlationId, CancellationToken ct)
    {
        // Initializing the saga state in the database.
        DataTransferSagaEntity saga = await InitializeSagaStateAsync(correlationId, ct);

        try
        {
            // 1. Initial -> Accessibility
            await ExecuteStepAsync(saga, TransferSagaStatus.AccessabilityCheck, accessStep, ct);

            // 2. Accessibility -> SchemaValidation
            await ExecuteStepAsync(saga, TransferSagaStatus.SchemaValidation, schemaStep, ct);

            // 3. SchemaValidation -> Transferring
            await ExecuteStepAsync(saga, TransferSagaStatus.Transferring, transferStep, ct);

            // 4. Final Success
            await FinalizeSagaAsync(saga, TransferSagaStatus.Completed, null, ct);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Saga {Id} was cancelled", correlationId);
            await FinalizeSagaAsync(saga, TransferSagaStatus.Cancelled, "Operation cancelled", ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Saga {Id} failed. Starting compensation...", correlationId);
            await compensationStep.ExecuteAsync(saga, ct);
            await FinalizeSagaAsync(saga, TransferSagaStatus.Failed, ex.Message, ct);
        }
    }

    public async Task<DataTransferSagaEntity> InitializeSagaStateAsync(Guid correlationId, CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        DataTransferDbContext dbContext = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        // Inbox message.
        InboxMessageEntity? message = await dbContext.InboxMessages.FirstOrDefaultAsync(x => x.CorrelationId == correlationId, ct)
            ?? throw new InvalidOperationException($"Unable to find inbox message by CorrelationId: {correlationId}");
        StartDataTransferCommand transferCommand = JsonSerializer.Deserialize<StartDataTransferCommand>(message.Payload)
            ?? throw new InvalidOperationException($"Unable to deserialize {nameof(StartDataTransferCommand)} from inbox message by CorrelationId: {correlationId}");

        // Check if there's already a similar saga (protection against duplicates from Inbox)
        DataTransferSagaEntity? saga = await dbContext.DataTransferSagas
            .FirstOrDefaultAsync(x => x.CorrelationId == correlationId, ct);

        if (saga is null)
        {
            saga = new DataTransferSagaEntity
            {
                CorrelationId = correlationId,
                CurrentState = TransferSagaStatus.Initial,
                SourceConnectionString = transferCommand.SourceConnectionString,
                TargetConnectionString = transferCommand.TargetConnectionString,
                SourceDatabaseType = transferCommand.SourceDatabaseType,
                TargetDatabaseType = transferCommand.TargetDatabaseType,
                TableName = transferCommand.TableName,
                UserUid = transferCommand.UserUid,
                LastUpdatedAt = DateTime.UtcNow
            };

            dbContext.DataTransferSagas.Add(saga);
            await dbContext.SaveChangesAsync(ct);
            logger.LogInformation("Saga {Id} initialized in DB", correlationId);
        }
        return saga;
    }

    private async Task ExecuteStepAsync(DataTransferSagaEntity saga, TransferSagaStatus status, ISagaStep step, CancellationToken ct)
    {
        await UpdateSagaStateWithOutboxAsync(saga, status, null, ct);
        await step.ExecuteAsync(saga, ct);
    }

    private async Task FinalizeSagaAsync(DataTransferSagaEntity saga, TransferSagaStatus status, string? error, CancellationToken ct)
    {
        await UpdateSagaStateWithOutboxAsync(saga, status, error, ct);
        logger.LogInformation("Saga {Id} finalized with status {Status}", saga.CorrelationId, status);
    }

    public async Task UpdateSagaStateWithOutboxAsync(
        DataTransferSagaEntity saga,
        TransferSagaStatus status,
        string? error,
        CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();
        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        // Update saga state.
        db.Attach(saga);
        saga.CurrentState = status;
        saga.LastUpdatedAt = DateTime.UtcNow;

        // If this is the first step after Initial, update the start time.
        if (status == TransferSagaStatus.AccessabilityCheck)
        {
            saga.StartedAt = DateTime.UtcNow;
        }

        // Write to Outbox
        Guid correlationId = saga.CorrelationId;
        db.OutboxMessages.Add(new OutboxMessageEntity
        {
            CorrelationId = correlationId,
            MessageType = nameof(DataTransferStatusUpdated),
            Topic = configuration[ConfigurationKeys.Services.Kafka.Topics.DataTransferStatusUpdates]!,
            UserUid = Guid.TryParse(saga.UserUid, out Guid userUid) ? userUid : null,
            Payload = JsonSerializer.Serialize(new DataTransferStatusUpdated(
                CorrelationId: correlationId,
                TransferStatus: status.ToTransferStatus().ToString(),
                InternalStatus: status.ToString(),
                Timestamp: DateTime.UtcNow,
                ErrorMessage: error)),
            CreatedAt = DateTime.UtcNow,
            Error = error
        });

        await db.SaveChangesAsync(ct);
    }
}
