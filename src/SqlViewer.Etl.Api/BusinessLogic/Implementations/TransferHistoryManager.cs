using SqlViewer.Etl.Api.BusinessLogic.Abstractions;
using SqlViewer.Etl.Api.Repositories.Abstractions;
using SqlViewer.Etl.Core.Data.Projections;
using SqlViewer.Shared.Dtos.Etl;
using SqlViewer.Shared.Messages.Etl.Commands;
using StackExchange.Redis;

namespace SqlViewer.Etl.Api.BusinessLogic.Implementations;

public sealed class TransferHistoryManager(
    ITransferHistoryRepository repository,
    IConnectionMultiplexer redis) : ITransferHistoryManager
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

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

    public async Task<BatchTransferStatusesResponseDto> GetStatusesAsync(
        Guid userUid,
        IEnumerable<Guid> correlationIds,
        CancellationToken ct = default)
    {
        IReadOnlyList<TransferJobDbProjection> statusProjections = await repository.GetStatusesAsync(userUid, correlationIds, ct);

        if (!statusProjections.Any())
            return new BatchTransferStatusesResponseDto();

        // Batch reading from Redis (MGET) is much faster than in a loop
        RedisKey[] keys = statusProjections
            .Select(p => (RedisKey)$"transfer:progress:{p.CorrelationId}")
            .ToArray();

        RedisValue[] progressValues = await _redisDb.StringGetAsync(keys);

        List<TransferStatusResponseDto> items = new(statusProjections.Count);
        for (int i = 0; i < statusProjections.Count; i++)
        {
            TransferJobDbProjection projection = statusProjections[i];
            RedisValue redisValue = progressValues[i];

            // We try to parse the progress; if it's not in Redis, set it to 0.
            double progress = redisValue.HasValue && double.TryParse(redisValue, out double p)
                ? p
                : (projection.IsFinalState ? 100.0 : 0.0);

            items.Add(new TransferStatusResponseDto
            {
                CorrelationId = projection.CorrelationId,
                StatusMessage = projection.CurrentStatus.ToString(),
                IsFinalState = projection.IsFinalState,
                Progress = progress
            });
        }

        return new BatchTransferStatusesResponseDto
        {
            Items = items
        };
    }

    public Task SaveTransferJobHistoryAsync(Guid correlationId, StartDataTransferCommand transferCommand) =>
        repository.SaveTransferJobHistoryAsync(correlationId, transferCommand);
}
