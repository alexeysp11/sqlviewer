using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Etl.Api.Tests.Integration.Infrastructure;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Etl.Api.Tests.Integration.Repositories.TransferHistoryRepositoryTests;

[Collection(nameof(PostgresCollection))]
public sealed class GetHistoryAsyncTests(PostgresFixture fixture)
{
    private async Task<EtlDbContext> CreateContextAsync()
    {
        EtlDbContext context = new(new DbContextOptionsBuilder<EtlDbContext>().UseNpgsql(fixture.ConnectionString).Options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    [Fact]
    public async Task GetHistoryAsync_ShouldReturnPagedData_UsingRawSql()
    {
        // Arrange
        using EtlDbContext context = await CreateContextAsync();
        TransferHistoryRepository repository = new(context);
        Guid userUid = Guid.NewGuid();

        context.TransferJobs.Add(new()
        {
            CorrelationId = Guid.NewGuid(),
            UserUid = userUid,
            CreatedAt = DateTime.UtcNow.AddMinutes(-1),
            SourceConnectionString = "src",
            TargetConnectionString = "dst",
            SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
            TargetDatabaseType = VelocipedeDatabaseType.SQLite
        });
        await context.SaveChangesAsync();

        // Act
        IEnumerable<TransferJobEntity> history = await repository.GetHistoryAsync(userUid, null, 10);

        // Assert
        history.Should().NotBeNullOrEmpty();
        history.First().UserUid.Should().Be(userUid);
    }

    [Fact]
    public async Task GetHistory_NoCorrelationId_ReturnsLatestJobs()
    {
        // Arrange
        using EtlDbContext context = await CreateContextAsync();
        TransferHistoryRepository repo = new(context);
        Guid userUid = Guid.NewGuid();

        context.TransferJobs.AddRange(
            new()
            {
                CorrelationId = Guid.NewGuid(),
                UserUid = userUid,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                SourceConnectionString = "s",
                TargetConnectionString = "t",
                SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
                TargetDatabaseType = VelocipedeDatabaseType.SQLite
            },
            new()
            {
                CorrelationId = Guid.NewGuid(),
                UserUid = userUid,
                CreatedAt = DateTime.UtcNow.AddMinutes(-5),
                SourceConnectionString = "s",
                TargetConnectionString = "t",
                SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
                TargetDatabaseType = VelocipedeDatabaseType.SQLite
            }
        );
        await context.SaveChangesAsync();

        // Act
        IEnumerable<TransferJobEntity> result = await repo.GetHistoryAsync(userUid, null, 10);

        // Assert
        result.Should().HaveCount(2);
        result.First().CreatedAt.Should().BeAfter(result.Last().CreatedAt);
    }

    [Fact]
    public async Task GetHistory_WithCorrelationId_ReturnsNextPageAfterCursor()
    {
        // Arrange
        using EtlDbContext context = await CreateContextAsync();
        TransferHistoryRepository repo = new(context);
        Guid userUid = Guid.NewGuid();
        Guid cursorId = Guid.NewGuid();

        TransferJobEntity oldJob = new()
        {
            CorrelationId = Guid.NewGuid(),
            UserUid = userUid,
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            SourceConnectionString = "s",
            TargetConnectionString = "t",
            SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
            TargetDatabaseType = VelocipedeDatabaseType.SQLite
        };
        TransferJobEntity cursorJob = new()
        {
            CorrelationId = cursorId,
            UserUid = userUid,
            CreatedAt = DateTime.UtcNow,
            SourceConnectionString = "s",
            TargetConnectionString = "t",
            SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
            TargetDatabaseType = VelocipedeDatabaseType.SQLite
        };
        TransferJobEntity newJob = new()
        {
            CorrelationId = Guid.NewGuid(),
            UserUid = userUid,
            CreatedAt = DateTime.UtcNow.AddHours(1),
            SourceConnectionString = "s",
            TargetConnectionString = "t",
            SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
            TargetDatabaseType = VelocipedeDatabaseType.SQLite
        };

        context.TransferJobs.AddRange(oldJob, cursorJob, newJob);
        await context.SaveChangesAsync();

        // Act
        IEnumerable<TransferJobEntity> result = await repo.GetHistoryAsync(userUid, cursorId, 10);

        // Assert
        result.Should().ContainSingle();
        result.First().CorrelationId.Should().Be(oldJob.CorrelationId);
    }

    [Fact]
    public async Task GetHistory_Limit_RespectsMaxCount()
    {
        // Arrange
        using EtlDbContext context = await CreateContextAsync();
        TransferHistoryRepository repo = new(context);
        Guid userUid = Guid.NewGuid();

        for (int i = 0; i < 5; i++)
        {
            context.TransferJobs.Add(new()
            {
                CorrelationId = Guid.NewGuid(),
                UserUid = userUid,
                SourceConnectionString = "s",
                TargetConnectionString = "t",
                SourceDatabaseType = VelocipedeDatabaseType.PostgreSQL,
                TargetDatabaseType = VelocipedeDatabaseType.SQLite,
            });
        }
        await context.SaveChangesAsync();

        // Act
        IEnumerable<TransferJobEntity> result = await repo.GetHistoryAsync(userUid, null, 2);

        // Assert
        result.Should().HaveCount(2);
    }
}
