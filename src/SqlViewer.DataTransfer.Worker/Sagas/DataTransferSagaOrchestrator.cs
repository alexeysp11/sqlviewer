using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Constants;
using SqlViewer.Shared.Messages.Etl.Commands;
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
    private const string SagaStatusUpdatedMessageType = "SagaStatusUpdated";

    public async Task ExecuteAsync(Guid correlationId, CancellationToken ct)
    {
        // Initializing the saga state in the database.
        await InitializeSagaStateAsync(correlationId, ct);

        try
        {
            // 1. Initial -> Accessibility
            await ExecuteStepAsync(correlationId, TransferSagaStatus.AccessabilityCheck, accessStep, ct);

            // 2. Accessibility -> SchemaValidation
            await ExecuteStepAsync(correlationId, TransferSagaStatus.SchemaValidation, schemaStep, ct);

            // 3. SchemaValidation -> Transferring
            await ExecuteStepAsync(correlationId, TransferSagaStatus.Transferring, transferStep, ct);

            // 4. Final Success
            await FinalizeSagaAsync(correlationId, TransferSagaStatus.Completed, null, ct);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Saga {Id} was cancelled", correlationId);
            await FinalizeSagaAsync(correlationId, TransferSagaStatus.Cancelled, "Operation cancelled", ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Saga {Id} failed. Starting compensation...", correlationId);
            await compensationStep.ExecuteAsync(correlationId, ct);
            await FinalizeSagaAsync(correlationId, TransferSagaStatus.Failed, ex.Message, ct);
        }
    }

    public async Task InitializeSagaStateAsync(Guid correlationId, CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        DataTransferDbContext dbContext = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();

        // Inbox message.
        InboxMessageEntity? message = await dbContext.InboxMessages.FirstOrDefaultAsync(x => x.CorrelationId == correlationId, ct)
            ?? throw new InvalidOperationException($"Unable to find inbox message by CorrelationId: {correlationId}");
        StartDataTransferCommand transferCommand = JsonSerializer.Deserialize<StartDataTransferCommand>(message.Payload)
            ?? throw new InvalidOperationException($"Unable to deserialize {nameof(StartDataTransferCommand)} from inbox message by CorrelationId: {correlationId}");

        // Check if there's already a similar saga (protection against duplicates from Inbox)
        DataTransferSagaStateEntity? existingSagaState = await dbContext.DataTransferSagaStates
            .FirstOrDefaultAsync(x => x.CorrelationId == correlationId, ct);

        if (existingSagaState == null)
        {
            DataTransferSagaStateEntity newSageState = new()
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

            dbContext.DataTransferSagaStates.Add(newSageState);
            await dbContext.SaveChangesAsync(ct);
            logger.LogInformation("Saga {Id} initialized in DB", correlationId);
        }
    }

    private async Task ExecuteStepAsync(Guid correlationId, TransferSagaStatus status, ISagaStep step, CancellationToken ct)
    {
        await UpdateSagaStateWithOutboxAsync(correlationId, status, null, ct);
        await step.ExecuteAsync(correlationId, ct);
    }

    private async Task FinalizeSagaAsync(Guid correlationId, TransferSagaStatus status, string? error, CancellationToken ct)
    {
        await UpdateSagaStateWithOutboxAsync(correlationId, status, error, ct);
        logger.LogInformation("Saga {Id} finalized with status {Status}", correlationId, status);
    }

    public async Task UpdateSagaStateWithOutboxAsync(Guid correlationId, TransferSagaStatus status, string? error, CancellationToken ct)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        DataTransferDbContext db = scope.ServiceProvider.GetRequiredService<DataTransferDbContext>();
        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        // Update SagaState
        DataTransferSagaStateEntity? state = await db.DataTransferSagaStates
            .FirstOrDefaultAsync(x => x.CorrelationId == correlationId, ct);

        if (state != null)
        {
            // Map enum to string for the CurrentState column
            state.CurrentState = status;
            state.LastUpdatedAt = DateTime.UtcNow;

            // If this is the first step after Initial, we record the start time
            if (status == TransferSagaStatus.AccessabilityCheck)
            {
                state.StartedAt = DateTime.UtcNow;
            }
        }

        // Write to Outbox
        db.OutboxMessages.Add(new OutboxMessageEntity
        {
            CorrelationId = correlationId,
            MessageType = SagaStatusUpdatedMessageType,
            Topic = configuration[ConfigurationKeys.Services.Kafka.Topics.DataTransferStatusUpdates]!,
            Payload = JsonSerializer.Serialize(new
            {
                CorrelationId = correlationId,
                Status = status.ToString(),
                ErrorMessage = error
            }),
            CreatedAt = DateTime.UtcNow,
            Error = error
        });

        await db.SaveChangesAsync(ct);
    }
}
