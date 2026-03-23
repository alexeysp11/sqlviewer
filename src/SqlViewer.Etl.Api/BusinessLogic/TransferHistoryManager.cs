using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Etl.Api.BusinessLogic;

public sealed class TransferHistoryManager(ITransferHistoryRepository repository) : ITransferHistoryManager
{
    public async Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit)
    {
        List<TransferJobDto> transferJobs = (await repository.GetHistoryAsync(userUid, correlationId, limit)).Select(e => new TransferJobDto
        {
            CorrelationId = e.CorrelationId,
            Source = e.Source,
            Target = e.Target,
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

    public Task SaveTransferJobHistoryAsync(Guid correlationId, StartTransferRequestDto requestDto) =>
        repository.SaveTransferJobHistoryAsync(correlationId, requestDto);
}
