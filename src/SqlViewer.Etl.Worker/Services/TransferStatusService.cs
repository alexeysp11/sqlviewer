using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Messages.Etl.Events;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Worker.Services;

public sealed class TransferStatusService(
    EtlDbContext dbContext,
    ILogger<TransferStatusService> logger) : ITransferStatusService
{
    public string MessageType => nameof(DataTransferStatusUpdated);

    public async Task ProcessAsync(InboxMessageEntity message, CancellationToken ct)
    {
        DataTransferStatusUpdated payload = JsonSerializer.Deserialize<DataTransferStatusUpdated>(message.Payload)
            ?? throw new InvalidOperationException("Failed to deserialize inbox payload.");

        // Find the corresponding transfer job by CorrelationId
        TransferJobEntity job = await dbContext.TransferJobs
            .FirstOrDefaultAsync(j => j.CorrelationId == message.CorrelationId, ct)
            ?? throw new InvalidOperationException($"TransferJob with CorrelationId {message.CorrelationId} not found.");

        // Map status from message to domain status
        TransferStatus newStatus = Enum.Parse<TransferStatus>(payload.TransferStatus);

        // 1. Update the job status
        job.CurrentStatus = newStatus;

        // 2. Add a new log entry
        TransferStatusLogEntity logEntry = new()
        {
            CorrelationId = message.CorrelationId,
            Status = newStatus,
            MetadataJson = message.Payload,
            Timestamp = payload.Timestamp
        };

        dbContext.TransferStatusLogs.Add(logEntry);

        await dbContext.SaveChangesAsync(ct);

        logger.LogInformation("Updated job {CorrelationId} status to {Status}", message.CorrelationId, newStatus);
    }
}
