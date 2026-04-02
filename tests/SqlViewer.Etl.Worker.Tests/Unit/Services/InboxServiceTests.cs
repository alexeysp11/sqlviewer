using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Worker.Services;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Worker.Tests.Unit.Services;

public sealed class InboxServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<ILogger<InboxService>> _loggerMock = new();

    [Fact]
    public async Task StoreMessageAsync_WhenMessageIsNew_ShouldSaveToDatabase()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using EtlDbContext dbContext = new(options);
        InboxService service = new(dbContext, _loggerMock.Object);
        InboxMessageEntity message = _fixture.Create<InboxMessageEntity>();

        // Act
        await service.StoreMessageAsync(message, CancellationToken.None);

        // Assert
        InboxMessageEntity? savedMessage = await dbContext.InboxMessages
            .FirstOrDefaultAsync(m => m.MessageId == message.MessageId);

        savedMessage.Should().NotBeNull();
        savedMessage!.CorrelationId.Should().Be(message.CorrelationId);
    }

    [Fact]
    public async Task StoreMessageAsync_WhenMessageAlreadyExists_ShouldNotSaveAndLogWarning()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using EtlDbContext dbContext = new(options);
        InboxMessageEntity existingMessage = _fixture.Create<InboxMessageEntity>();
        dbContext.InboxMessages.Add(existingMessage);
        await dbContext.SaveChangesAsync();

        InboxService service = new(dbContext, _loggerMock.Object);

        // Create new entity with same MessageId
        InboxMessageEntity duplicateMessage = _fixture.Build<InboxMessageEntity>()
            .With(m => m.MessageId, existingMessage.MessageId)
            .Create();

        // Act
        await service.StoreMessageAsync(duplicateMessage, CancellationToken.None);

        // Assert
        int count = await dbContext.InboxMessages.CountAsync();
        count.Should().Be(1); // No new records added

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("already exists")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}