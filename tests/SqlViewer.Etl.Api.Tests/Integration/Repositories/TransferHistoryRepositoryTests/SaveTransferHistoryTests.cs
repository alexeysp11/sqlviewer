using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Etl.Api.Tests.Integration.Infrastructure;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Data.Entities;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Messages.Etl.Commands;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace SqlViewer.Etl.Api.Tests.Integration.Repositories.TransferHistoryRepositoryTests;

[Collection(nameof(PostgresCollection))]
public sealed class SaveTransferHistoryTests(PostgresFixture fixture)
{
    private readonly Fixture _autoFixture = new();

    private async Task<EtlDbContext> CreateContextAsync()
    {
        EtlDbContext context = new(new DbContextOptionsBuilder<EtlDbContext>().UseNpgsql(fixture.ConnectionString).Options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    [Fact]
    public async Task Save_ValidDto_PersistsCorrectData()
    {
        // Arrange
        Guid correlationId = _autoFixture.Create<Guid>();
        Guid userUid = _autoFixture.Create<Guid>();

        using EtlDbContext context = await CreateContextAsync();
        TransferHistoryRepository repo = new(context);

        StartDataTransferCommand transferCommand = new(
            CorrelationId: correlationId,
            UserUid: userUid.ToString(),
            SourceConnectionString: _autoFixture.Create<string>(),
            TargetConnectionString: _autoFixture.Create<string>(),
            SourceDatabaseType: _autoFixture.Create<VelocipedeDatabaseType>(),
            TargetDatabaseType: _autoFixture.Create<VelocipedeDatabaseType>(),
            TableName: _autoFixture.Create<string>());

        var expected = new
        {
            CorrelationId = transferCommand.CorrelationId,
            UserUid = Guid.Parse(transferCommand.UserUid),
            SourceConnectionString = transferCommand.SourceConnectionString,
            TargetConnectionString = transferCommand.TargetConnectionString,
            SourceDatabaseType = transferCommand.SourceDatabaseType,
            TargetDatabaseType = transferCommand.TargetDatabaseType,
            TableName = transferCommand.TableName,
            CurrentStatus = TransferStatus.None
        };

        // Act
        await repo.SaveTransferJobHistoryAsync(correlationId, transferCommand);

        // Assert
        using EtlDbContext assertContext = await CreateContextAsync();
        TransferJobEntity? saved = await assertContext.TransferJobs
            .FirstOrDefaultAsync(x => x.CorrelationId == correlationId);

        saved.Should().NotBeNull();
        saved.Should().BeEquivalentTo(expected, options => options.ExcludingMissingMembers());
        saved!.CurrentStatus.Should().Be(TransferStatus.None);
        saved.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("not-a-guid")]
    [InlineData("")]
    public async Task Save_InvalidUserUid_ThrowsException(string invalidUserUid)
    {
        // Arrange
        Guid correlationId = _autoFixture.Create<Guid>();

        using EtlDbContext context = await CreateContextAsync();
        TransferHistoryRepository repo = new(context);

        StartDataTransferCommand transferCommand = new(
            CorrelationId: correlationId,
            UserUid: invalidUserUid,
            SourceConnectionString: _autoFixture.Create<string>(),
            TargetConnectionString: _autoFixture.Create<string>(),
            SourceDatabaseType: _autoFixture.Create<VelocipedeDatabaseType>(),
            TargetDatabaseType: _autoFixture.Create<VelocipedeDatabaseType>(),
            TableName: _autoFixture.Create<string>());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            repo.SaveTransferJobHistoryAsync(Guid.NewGuid(), transferCommand));
    }
}
