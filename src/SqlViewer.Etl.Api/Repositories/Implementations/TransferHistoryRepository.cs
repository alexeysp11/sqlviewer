using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Api.Repositories.Abstractions;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Data.Projections;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Messages.Etl.Commands;

namespace SqlViewer.Etl.Api.Repositories.Implementations;

public sealed class TransferHistoryRepository(EtlDbContext dbContext) : ITransferHistoryRepository
{
    public async Task<IEnumerable<TransferJobEntity>> GetHistoryAsync(Guid userUid, Guid? correlationId, int limit)
    {
        using DbConnection connection = dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        const string sql = @"
WITH cursor_record AS (
    SELECT ""CreatedAt"", ""Id""
    FROM ""TransferJobs""
    WHERE ""CorrelationId"" = @LastCorrelationId AND ""UserUid"" = @OwnerUserUid
    LIMIT 1
)
SELECT
    ""Id"",
    ""CorrelationId"",
    ""UserUid"",
    ""CurrentStatus"",
    ""SourceConnectionString"",
    ""TargetConnectionString"",
    ""SourceDatabaseType"",
    ""TargetDatabaseType"",
    ""TableName"",
    ""CreatedAt""
FROM ""TransferJobs""
WHERE
    ""UserUid"" = @OwnerUserUid
    AND (
        @LastCorrelationId IS NULL
        OR
        ""CreatedAt"" < (SELECT ""CreatedAt"" FROM cursor_record)
        OR
        (""CreatedAt"" = (SELECT ""CreatedAt"" FROM cursor_record) AND ""Id"" < (SELECT ""Id"" FROM cursor_record))
    )
ORDER BY ""CreatedAt"" DESC, ""Id"" DESC
LIMIT @Limit;";

        return await connection.QueryAsync<TransferJobEntity>(sql, new
        {
            OwnerUserUid = userUid,
            LastCorrelationId = correlationId,
            Limit = limit
        });
    }

    public async Task<IReadOnlyList<TransferJobDbProjection>> GetStatusesAsync(
        Guid userUid,
        IEnumerable<Guid> correlationIds,
        CancellationToken ct)
    {
        DbConnection connection = dbContext.Database.GetDbConnection();

        string sql = @"
SELECT 
    ""CorrelationId"",
    ""CurrentStatus"",
    CASE 
        WHEN ""CurrentStatus"" IN (3, 4, 5, 7, 8) THEN TRUE 
        ELSE FALSE 
    END AS ""IsFinalState""
FROM ""TransferJobs""
WHERE ""UserUid"" = @OwnerUserUid 
    AND ""CorrelationId"" = ANY(@CorrelationIds);";

        var parameters = new
        {
            OwnerUserUid = userUid,
            CorrelationIds = correlationIds.ToArray()
        };

        IEnumerable<TransferJobDbProjection> result = await connection.QueryAsync<TransferJobDbProjection>(
            new CommandDefinition(sql, parameters, cancellationToken: ct)
        );

        return result.ToList();
    }

    public async Task SaveTransferJobHistoryAsync(Guid correlationId, StartDataTransferCommand transferCommand)
    {
        if (!Guid.TryParse(transferCommand.UserUid, out Guid userUid))
            throw new InvalidOperationException($"Unable to save transfer job history, invalid user UID: {transferCommand.UserUid}");

        await dbContext.TransferJobs.AddAsync(new()
        {
            CorrelationId = correlationId,
            UserUid = userUid,
            SourceConnectionString = transferCommand.SourceConnectionString,
            TargetConnectionString = transferCommand.TargetConnectionString,
            SourceDatabaseType = transferCommand.SourceDatabaseType,
            TargetDatabaseType = transferCommand.TargetDatabaseType,
            TableName = transferCommand.TableName,
            CurrentStatus = TransferStatus.None,
        });
        await dbContext.SaveChangesAsync();
    }
}
