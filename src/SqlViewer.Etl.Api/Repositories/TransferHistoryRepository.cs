using System.Data.Common;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;

namespace SqlViewer.Etl.Api.Repositories;

public class TransferHistoryRepository(
    ILogger<TransferHistoryRepository> logger,
    EtlDbContext dbContext) : ITransferHistoryRepository
{
    public async Task<IEnumerable<TransferJobEntity>> GetHistoryAsync(
        Guid userUid,
        DateTime? cursorAt,
        long? cursorId,
        int limit)
    {
        using DbConnection connection = dbContext.Database.GetDbConnection();

        logger.LogInformation("User {UserUid} requested history. Cursor: {Cursor}, Limit: {Limit}",
            userUid, cursorId ?? 0, limit);

        // Используем эффективный фильтр для комбинированного курсора
        const string sql = @"
            SELECT * FROM ""TransferJobs""
            WHERE ""UserUid"" = @UserUid
            AND (
                @CursorAt IS NULL OR 
                ""CreatedAt"" < @CursorAt OR 
                (""CreatedAt"" = @CursorAt AND ""Id"" < @CursorId)
            )
            ORDER BY ""CreatedAt"" DESC, ""Id"" DESC
            LIMIT @Limit;";

        return await connection.QueryAsync<TransferJobEntity>(sql, new
        {
            UserUid = userUid,
            CursorAt = cursorAt,
            CursorId = cursorId,
            Limit = limit
        });
    }
}
