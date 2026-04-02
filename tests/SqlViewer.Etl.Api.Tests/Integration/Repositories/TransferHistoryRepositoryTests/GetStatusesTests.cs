using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Api.Repositories.Implementations;
using SqlViewer.Etl.Api.Tests.Integration.Infrastructure;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Data.Projections;
using SqlViewer.Etl.Core.Enums;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Etl.Api.Tests.Integration.Repositories.TransferHistoryRepositoryTests;

[Collection(nameof(PostgresCollection))]
public sealed class GetStatusesTests(PostgresFixture fixture)
{
    private readonly Fixture _autoFixture = new();

    private async Task<EtlDbContext> CreateContextAsync()
    {
        EtlDbContext context = new(new DbContextOptionsBuilder<EtlDbContext>().UseNpgsql(fixture.ConnectionString).Options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    [Fact]
    public async Task GetStatusesAsync_ShouldReturnCorrectData_WhenRecordsExist()
    {
        // Arrange
        Guid userUid = Guid.NewGuid();
        Guid correlationId1 = Guid.NewGuid();
        Guid correlationId2 = Guid.NewGuid();

        using EtlDbContext dbContext = await CreateContextAsync();

        // Prepare test entities
        List<TransferJobEntity> jobs =
        [
            new()
            {
                CorrelationId = correlationId1,
                UserUid = userUid,
                CurrentStatus = TransferStatus.Completed, // FinalState = true
                SourceConnectionString = _autoFixture.Create<string>(),
                TargetConnectionString = _autoFixture.Create<string>(),
                SourceDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TargetDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TableName = _autoFixture.Create<string>()
            },
            new()
            {
                CorrelationId = correlationId2,
                UserUid = userUid,
                CurrentStatus = TransferStatus.Started, // FinalState = false
                SourceConnectionString = _autoFixture.Create<string>(),
                TargetConnectionString = _autoFixture.Create<string>(),
                SourceDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TargetDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
                TableName = _autoFixture.Create<string>()
            }
        ];

        await dbContext.TransferJobs.AddRangeAsync(jobs);
        await dbContext.SaveChangesAsync();

        List<Guid> idsToFetch = [correlationId1, correlationId2];


        // Act
        TransferHistoryRepository repository = new(dbContext);
        IReadOnlyList<TransferJobDbProjection> result = await repository.GetStatusesAsync(
            userUid,
            idsToFetch,
            CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        TransferJobDbProjection job1 = result.First(x => x.CorrelationId == correlationId1);
        job1.CurrentStatus.Should().Be(TransferStatus.Completed);
        job1.IsFinalState.Should().BeTrue();

        TransferJobDbProjection job2 = result.First(x => x.CorrelationId == correlationId2);
        job2.CurrentStatus.Should().Be(TransferStatus.Started);
        job2.IsFinalState.Should().BeFalse();
    }

    [Fact]
    public async Task GetStatusesAsync_ShouldReturnEmpty_WhenUserUidDoesNotMatch()
    {
        // Arrange
        Guid actualUserUid = Guid.NewGuid();
        Guid wrongUserUid = Guid.NewGuid();
        Guid correlationId = Guid.NewGuid();

        using EtlDbContext dbContext = await CreateContextAsync();

        TransferJobEntity job = new()
        {
            CorrelationId = correlationId,
            UserUid = actualUserUid,
            CurrentStatus = TransferStatus.Queued,
            SourceConnectionString = _autoFixture.Create<string>(),
            TargetConnectionString = _autoFixture.Create<string>(),
            SourceDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
            TargetDatabaseType = _autoFixture.Create<VelocipedeDatabaseType>(),
            TableName = _autoFixture.Create<string>()
        };

        await dbContext.TransferJobs.AddAsync(job);
        await dbContext.SaveChangesAsync();

        // Act
        TransferHistoryRepository repository = new(dbContext);
        IReadOnlyList<TransferJobDbProjection> result = await repository.GetStatusesAsync(
            wrongUserUid,
            [correlationId],
            CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetStatusesAsync_ShouldThrowOperationCanceledException_WhenTokenIsCancelled()
    {
        // Arrange
        Guid userUid = Guid.NewGuid();
        List<Guid> correlationIds = [Guid.NewGuid()];

        // Create a cancellation token that is already in the canceled state
        CancellationTokenSource cts = new();
        await cts.CancelAsync();
        CancellationToken cancelledToken = cts.Token;

        using EtlDbContext dbContext = await CreateContextAsync();
        TransferHistoryRepository repository = new(dbContext);

        // Act & Assert
        // We expect the operation to throw TaskCanceledException or OperationCanceledException
        // when the token is passed to the underlying database command
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            repository.GetStatusesAsync(userUid, correlationIds, cancelledToken));
    }
}
