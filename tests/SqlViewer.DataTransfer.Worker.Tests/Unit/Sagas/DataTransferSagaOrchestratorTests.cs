using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;
using SqlViewer.DataTransfer.Worker.Sagas;
using SqlViewer.Etl.Core.Enums;
using FluentAssertions;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Sagas;

public sealed class DataTransferSagaOrchestratorTests : IDisposable
{
    private readonly DataTransferDbContext _db;
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<ILogger<DataTransferSagaOrchestrator>> _loggerMock = new();

    private readonly Mock<AccessabilityCheckStep> _accessStepMock;
    private readonly Mock<SchemaValidationStep> _schemaStepMock;
    private readonly Mock<DataTransferStep> _transferStepMock;
    private readonly Mock<CompensationStep> _compensationStepMock;

    private readonly DataTransferSagaOrchestrator _orchestrator;
    private readonly Fixture _fixture = new();

    public DataTransferSagaOrchestratorTests()
    {
        // In-Memory database.
        DbContextOptions<DataTransferDbContext> options = new DbContextOptionsBuilder<DataTransferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new DataTransferDbContext(options);

        // Mocks.
        _accessStepMock = new Mock<AccessabilityCheckStep>(new Mock<ILogger<AccessabilityCheckStep>>().Object);
        _schemaStepMock = new Mock<SchemaValidationStep>(new Mock<ILogger<SchemaValidationStep>>().Object);
        _transferStepMock = new Mock<DataTransferStep>(new Mock<ILogger<DataTransferStep>>().Object, new Mock<IDistributedCache>().Object);
        _compensationStepMock = new Mock<CompensationStep>(new Mock<ILogger<CompensationStep>>().Object);

        // DI Scope.
        Mock<IServiceScope> scopeMock = new();
        Mock<IServiceProvider> serviceProviderMock = new();

        _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
        scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(DataTransferDbContext))).Returns(_db);

        _orchestrator = new DataTransferSagaOrchestrator(
            _scopeFactoryMock.Object,
            _loggerMock.Object,
            _accessStepMock.Object,
            _schemaStepMock.Object,
            _transferStepMock.Object,
            _compensationStepMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenStepFails_ShouldInvokeCompensationAndSetFailedStatus()
    {
        // Arrange
        Guid correlationId = Guid.NewGuid();
        DataTransferSagaStateEntity sagaState = _fixture.Build<DataTransferSagaStateEntity>()
            .With(x => x.CorrelationId, correlationId)
            .Create();
        _db.DataTransferSagaStates.Add(sagaState);
        await _db.SaveChangesAsync();

        // Simulate an error in the second step.
        _accessStepMock.Setup(x => x.ExecuteAsync(correlationId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _schemaStepMock.Setup(x => x.ExecuteAsync(correlationId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB Error"));

        // Act
        await _orchestrator.ExecuteAsync(correlationId, CancellationToken.None);

        // Assert
        // 1. Checking the compensation call.
        _compensationStepMock.Verify(x => x.ExecuteAsync(correlationId, It.IsAny<CancellationToken>()), Times.Once);

        // 2. Checking the status in the database.
        DataTransferSagaStateEntity updatedState = await _db.DataTransferSagaStates.FirstAsync(x => x.CorrelationId == correlationId);
        updatedState.CurrentState.Should().Be(TransferSagaStatus.Failed.ToString());
    }

    [Fact]
    public async Task UpdateSagaStateWithOutboxAsync_ShouldUpdateDbAndAddOutboxMessage()
    {
        // Arrange
        Guid correlationId = Guid.NewGuid();
        DataTransferSagaStateEntity sagaState = _fixture.Build<DataTransferSagaStateEntity>()
            .With(x => x.CorrelationId, correlationId)
            .With(x => x.CurrentState, "Initial")
            .Create();
        _db.DataTransferSagaStates.Add(sagaState);
        await _db.SaveChangesAsync();

        // Act
        await _orchestrator.UpdateSagaStateWithOutboxAsync(correlationId, TransferSagaStatus.Transferring, "Error", CancellationToken.None);

        // Assert
        DataTransferSagaStateEntity updatedState = await _db.DataTransferSagaStates.FirstAsync(x => x.CorrelationId == correlationId);
        updatedState.CurrentState.Should().Be("Transferring");

        OutboxMessageEntity? outboxMessage = await _db.OutboxMessages.FirstOrDefaultAsync(x => x.CorrelationId == correlationId);
        outboxMessage.Should().NotBeNull();
        outboxMessage!.MessageType.Should().Be("SagaStatusUpdated");
        outboxMessage.Payload.Should().Contain("Transferring");
    }

    public void Dispose() => _db.Dispose();
}
