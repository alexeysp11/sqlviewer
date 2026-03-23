using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SqlViewer.Etl.Api.Repositories;
using SqlViewer.Etl.Api.Tests.Integration.Infrastructure;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Dtos.Etl;

namespace SqlViewer.Etl.Api.Tests.Integration.Repositories.TransferHistoryRepositoryTests;

[Collection(nameof(PostgresCollection))]
public sealed class SaveTransferHistoryTests(PostgresFixture fixture)
{
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
        using EtlDbContext context = await CreateContextAsync();
        TransferHistoryRepository repo = new(context);
        Guid correlationId = Guid.NewGuid();
        Guid userUid = Guid.NewGuid();
        StartTransferRequestDto dto = new()
        {
            UserUid = userUid.ToString(),
            SourceConnectionString = "src_conn",
            TargetConnectionString = "target_conn",
            TableName = "Test",
        };

        // Act
        await repo.SaveTransferJobHistoryAsync(correlationId, dto);

        // Assert
        Core.Data.Entities.TransferJobEntity? saved = await context.TransferJobs.FirstOrDefaultAsync(x => x.CorrelationId == correlationId);
        saved.Should().NotBeNull();
        saved!.UserUid.Should().Be(userUid);
        saved.Source.Should().Be("src_conn");
        saved.CurrentStatus.Should().Be(TransferStatus.None);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("not-a-guid")]
    [InlineData("")]
    public async Task Save_InvalidUserUid_ThrowsException(string invalidUid)
    {
        // Arrange
        using EtlDbContext context = await CreateContextAsync();
        TransferHistoryRepository repo = new(context);
        StartTransferRequestDto dto = new()
        {
            UserUid = invalidUid,
            SourceConnectionString = "src",
            TargetConnectionString = "target",
            TableName = "table"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            repo.SaveTransferJobHistoryAsync(Guid.NewGuid(), dto));
    }
}
