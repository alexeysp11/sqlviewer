using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Services;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Services;

public sealed class InboxServiceTests : IDisposable
{
    private readonly DataTransferDbContext _db;
    private readonly Mock<ILogger<InboxService>> _loggerMock;
    private readonly InboxService _service;
    private readonly Fixture _autoFixture;

    public InboxServiceTests()
    {
        _autoFixture = new Fixture();
        _autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());

        DbContextOptions<DataTransferDbContext> options = new DbContextOptionsBuilder<DataTransferDbContext>()
            .UseInMemoryDatabase(_autoFixture.Create<string>())
            .Options;
        _db = new DataTransferDbContext(options);
        _loggerMock = new Mock<ILogger<InboxService>>();
        _service = new InboxService(_db, _loggerMock.Object);
    }

    [Fact]
    public async Task StoreMessageAsync_WhenMessageIsNew_ShouldSaveToDatabase()
    {
        // Arrange
        InboxMessageEntity message = _autoFixture.Create<InboxMessageEntity>();

        // Act
        await _service.StoreMessageAsync(message, CancellationToken.None);

        // Assert
        _db.InboxMessages.Should().ContainSingle(m => m.CorrelationId == message.CorrelationId);
    }

    [Fact]
    public async Task StoreMessageAsync_WhenMessageAlreadyExists_ShouldLogWarningAndNotDuplicate()
    {
        // Arrange
        InboxMessageEntity existingMessage = _autoFixture.Create<InboxMessageEntity>();
        _db.InboxMessages.Add(existingMessage);
        await _db.SaveChangesAsync();

        InboxMessageEntity duplicateMessage = _autoFixture.Build<InboxMessageEntity>()
            .With(m => m.CorrelationId, existingMessage.CorrelationId)
            .Create();

        // Act
        await _service.StoreMessageAsync(duplicateMessage, CancellationToken.None);

        // Assert
        _db.InboxMessages.Should().HaveCount(1);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("already exists")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    public void Dispose() => _db?.Dispose();
}
