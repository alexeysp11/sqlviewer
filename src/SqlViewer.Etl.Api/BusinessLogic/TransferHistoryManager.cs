using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.Shared.Helpers.DataTransfer;

namespace SqlViewer.Etl.Api.BusinessLogic;

public class TransferHistoryManager(ITransferHistoryRepository repository) : ITransferHistoryManager
{
    public async Task<TransferHistoryResponseDto> GetHistoryAsync(Guid userUid, string? cursorToken, int limit)
    {
        // 1. Decode the cursor
        (DateTime CreatedAt, long Id)? cursor = DataTransferCursorHelper.DecodeCursor(cursorToken);

        // 2. Getting data from the repository
        List<TransferJobEntity> entities = (await repository.GetHistoryAsync(
            userUid,
            cursor?.CreatedAt,
            cursor?.Id,
            limit)).ToList();
        List<TransferJobDto> dtos = entities.Select(e => new TransferJobDto
        {
            CorrelationId = e.CorrelationId,
            Source = e.Source,
            Target = e.Target,
            Status = e.CurrentStatus.ToString(),
            Time = e.CreatedAt
        }).ToList();

        // 3. Generate a token for the next page if the full limit is returned.
        string? nextCursor = null;
        if (entities.Count == limit)
        {
            TransferJobEntity lastItem = entities.Last();
            nextCursor = DataTransferCursorHelper.EncodeCursor(lastItem.CreatedAt, lastItem.Id);
        }

        return new TransferHistoryResponseDto
        {
            Items = dtos,
            NextCursor = nextCursor
        };
    }
}
