using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Messages.Etl.Commands;

namespace SqlViewer.DataTransfer.Worker.Services.Abstractions;

/// <summary>
/// Manages the state and execution flow of data transfer sagas.
/// </summary>
public interface ISagaService
{
    /// <summary>
    /// Initializes a new saga state based on the incoming command.
    /// </summary>
    Task<DataTransferSagaStateEntity> CreateSagaAsync(StartDataTransferCommand command, CancellationToken ct);

    /// <summary>
    /// Updates the current status and progress of an existing saga.
    /// </summary>
    Task UpdateSagaStatusAsync(Guid correlationId, TransferSagaStatus status, string? error = null, CancellationToken ct = default);

    /// <summary>
    /// Updates technical execution details like progress percentage or rows processed.
    /// </summary>
    Task UpdateProgressAsync(Guid correlationId, double progress, int rowsProcessed, CancellationToken ct = default);
}
