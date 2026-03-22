using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;

namespace SqlViewer.Etl.Api.Repositories;

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
SELECT ""Id"", ""CorrelationId"", ""CurrentStatus"", ""Source"", ""Target"", ""CreatedAt""
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
}
