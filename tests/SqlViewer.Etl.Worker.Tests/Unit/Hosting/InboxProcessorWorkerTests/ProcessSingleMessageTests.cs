using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.Etl.Core.Data.DbContexts;
using SqlViewer.Etl.Worker.Hosting;
using SqlViewer.Etl.Worker.Services;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Worker.Tests.Unit.Hosting.InboxProcessorWorkerTests;

public sealed class ProcessSingleMessageTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<ITransferStatusService> _handlerMock = new();

    [Fact]
    public async Task ProcessSingleMessageAsync_WhenSuccessful_ShouldInvokeHandlerAndDeleteMessage()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using EtlDbContext db = new(options);

        InboxMessageEntity message = _fixture.Create<InboxMessageEntity>();
        db.InboxMessages.Add(message);
        await db.SaveChangesAsync();

        _handlerMock.Setup(h => h.MessageType).Returns(message.MessageType);
        SetupScopeFactory(db, transferStatusService: _handlerMock.Object);

        InboxProcessorWorker worker = CreateWorker();

        // Act
        await worker.ProcessSingleMessageAsync(message, CancellationToken.None);

        // Assert
        _handlerMock.Verify(h => h.ProcessAsync(It.IsAny<InboxMessageEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        db.InboxMessages.Any(m => m.Id == message.Id).Should().BeFalse(); // Message deleted
    }

    [Fact]
    public async Task ProcessSingleMessageAsync_WhenNoHandlerFound_ShouldLogWarningAndReturn()
    {
        // Arrange
        DbContextOptions<EtlDbContext> options = new DbContextOptionsBuilder<EtlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        using EtlDbContext db = new(options);
        InboxMessageEntity message = _fixture.Create<InboxMessageEntity>();

        SetupScopeFactory(db, transferStatusService: null);
        InboxProcessorWorker worker = CreateWorker();

        // Act
        await worker.ProcessSingleMessageAsync(message, CancellationToken.None);

        // Assert
        _handlerMock.Verify(h => h.ProcessAsync(It.IsAny<InboxMessageEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private void SetupScopeFactory(EtlDbContext db, ITransferStatusService? transferStatusService)
    {
        Mock<IServiceScope> scopeMock = new();
        scopeMock.Setup(s => s.ServiceProvider.GetService(typeof(EtlDbContext))).Returns(db);
        scopeMock.Setup(s => s.ServiceProvider.GetService(typeof(ITransferStatusService))).Returns(transferStatusService);
        _scopeFactoryMock.Setup(s => s.CreateScope()).Returns(scopeMock.Object);
    }

    private InboxProcessorWorker CreateWorker() =>
        new(_scopeFactoryMock.Object, Mock.Of<ILogger<InboxProcessorWorker>>());
}
