using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.Shared.Messages.Etl.Commands;

namespace SqlViewer.Etl.Api.BusinessLogic;

public sealed class TransferHistoryManager(ITransferHistoryRepository repository) : ITransferHistoryManager
{
    public async Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit)
    {
        List<TransferJobDto> transferJobs = (await repository.GetHistoryAsync(userUid, correlationId, limit)).Select(e => new TransferJobDto
        {
            CorrelationId = e.CorrelationId,
            SourceConnectionString = e.SourceConnectionString,
            TargetConnectionString = e.TargetConnectionString,
            SourceDatabaseType = e.SourceDatabaseType,
            TargetDatabaseType = e.TargetDatabaseType,
            TableName = e.TableName,
            Status = e.CurrentStatus.ToString(),
            Time = e.CreatedAt
        }).ToList();

        Guid? cursorCorrelationId = transferJobs.LastOrDefault()?.CorrelationId;
        return new TransferHistoryResponseDto
        {
            Items = transferJobs,
            CursorCorrelationId = cursorCorrelationId
        };
    }

    public Task SaveTransferJobHistoryAsync(Guid correlationId, StartDataTransferCommand transferCommand) =>
        repository.SaveTransferJobHistoryAsync(correlationId, transferCommand);
}
