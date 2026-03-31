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

        if (!Enum.TryParse(payload.TransferStatus, true, out TransferStatus newStatus)) return;

        // Update only if the new status is more important than the current one.
        if (TransferStatusExtensions.GetStatusRank(newStatus) > TransferStatusExtensions.GetStatusRank(job.CurrentStatus))
        {
            job.CurrentStatus = newStatus;
            logger.LogInformation("Job {CorrelationId} status updated to {Status}", message.CorrelationId, newStatus);
        }
        else
        {
            logger.LogWarning("Job {CorrelationId} current status {Current} is more significant than incoming {Incoming}. Main status not changed.",
                message.CorrelationId, job.CurrentStatus, newStatus);
        }

        TransferStatusLogEntity logEntry = new()
        {
            CorrelationId = message.CorrelationId,
            Status = newStatus,
            MetadataJson = message.Payload,
            Timestamp = payload.Timestamp
        };
        dbContext.TransferStatusLogs.Add(logEntry);

        await dbContext.SaveChangesAsync(ct);
    }
}
