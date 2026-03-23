using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Etl.Api.Tests.Integration.Infrastructure;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;

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
            Source = "src",
            Target = "dst"
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
            new() { CorrelationId = Guid.NewGuid(), UserUid = userUid, CreatedAt = DateTime.UtcNow.AddMinutes(-10), Source = "s", Target = "t" },
            new() { CorrelationId = Guid.NewGuid(), UserUid = userUid, CreatedAt = DateTime.UtcNow.AddMinutes(-5), Source = "s", Target = "t" }
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

        TransferJobEntity oldJob = new() { CorrelationId = Guid.NewGuid(), UserUid = userUid, CreatedAt = DateTime.UtcNow.AddHours(-1), Source = "s", Target = "t" };
        TransferJobEntity cursorJob = new() { CorrelationId = cursorId, UserUid = userUid, CreatedAt = DateTime.UtcNow, Source = "s", Target = "t" };
        TransferJobEntity newJob = new() { CorrelationId = Guid.NewGuid(), UserUid = userUid, CreatedAt = DateTime.UtcNow.AddHours(1), Source = "s", Target = "t" };

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
            context.TransferJobs.Add(new() { CorrelationId = Guid.NewGuid(), UserUid = userUid, Source = "s", Target = "t" });
        await context.SaveChangesAsync();

        // Act
        IEnumerable<TransferJobEntity> result = await repo.GetHistoryAsync(userUid, null, 2);

        // Assert
        result.Should().HaveCount(2);
    }
}
