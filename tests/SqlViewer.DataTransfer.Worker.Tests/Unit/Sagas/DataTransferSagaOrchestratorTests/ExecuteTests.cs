using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Sagas;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Messages.Etl.Commands;
using SqlViewer.Shared.Messages.Storage.Entities;
using StackExchange.Redis;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Sagas.DataTransferSagaOrchestratorTests;

public sealed class ExecuteTests : IDisposable
{
    private readonly DataTransferDbContext _db;
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock = new();
    private readonly Mock<ILogger<DataTransferSagaOrchestrator>> _loggerMock = new();
    private readonly Mock<IConfiguration> _configMock = new();

    private readonly Mock<AccessabilityCheckStep> _accessStepMock;
    private readonly Mock<SchemaValidationStep> _schemaStepMock;
    private readonly Mock<DataTransferStep> _transferStepMock;
    private readonly Mock<CompensationStep> _compensationStepMock;

    private readonly DataTransferSagaOrchestrator _orchestrator;
    private readonly Fixture _fixture = new();

    public ExecuteTests()
    {
        // In-Memory database.
        DbContextOptions<DataTransferDbContext> options = new DbContextOptionsBuilder<DataTransferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new DataTransferDbContext(options);

        _configMock.Setup(c => c[It.IsAny<string>()]).Returns(_fixture.Create<string>());

        // DI Scope.
        Mock<IServiceScope> scopeMock = new();
        Mock<IServiceProvider> serviceProviderMock = new();

        _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
        scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(DataTransferDbContext))).Returns(_db);
        serviceProviderMock.Setup(x => x.GetService(typeof(IConfiguration))).Returns(_configMock.Object);

        // Mocks.
        _accessStepMock = new Mock<AccessabilityCheckStep>(new Mock<ILogger<AccessabilityCheckStep>>().Object);
        _schemaStepMock = new Mock<SchemaValidationStep>(new Mock<ILogger<SchemaValidationStep>>().Object);
        _transferStepMock = new Mock<DataTransferStep>(new Mock<ILogger<DataTransferStep>>().Object, _scopeFactoryMock.Object, new Mock<IConnectionMultiplexer>().Object);
        _compensationStepMock = new Mock<CompensationStep>(new Mock<ILogger<CompensationStep>>().Object);

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
        Guid correlationId = _fixture.Create<Guid>();
        StartDataTransferCommand transferCommand = _fixture.Build<StartDataTransferCommand>()
            .With(x => x.CorrelationId, correlationId)
            .Create();
        InboxMessageEntity inboxMessage = _fixture.Build<InboxMessageEntity>()
            .With(x => x.CorrelationId, correlationId)
            .With(x => x.Payload, JsonSerializer.Serialize(transferCommand))
            .Create();
        DataTransferSagaEntity transferSaga = _fixture.Build<DataTransferSagaEntity>()
            .With(x => x.CorrelationId, correlationId)
            .Create();
        _db.InboxMessages.Add(inboxMessage);
        _db.DataTransferSagas.Add(transferSaga);
        await _db.SaveChangesAsync();

        // Simulate an error in the second step.
        _accessStepMock.Setup(x => x.ExecuteAsync(transferSaga, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _schemaStepMock.Setup(x => x.ExecuteAsync(transferSaga, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB Error"));

        // Act
        await _orchestrator.ExecuteAsync(correlationId, CancellationToken.None);

        // Assert
        // 1. Checking the compensation call.
        _compensationStepMock.Verify(x => x.ExecuteAsync(transferSaga, It.IsAny<CancellationToken>()), Times.Once);

        // 2. Checking the status in the database.
        DataTransferSagaEntity updatedSaga = await _db.DataTransferSagas.FirstAsync(x => x.CorrelationId == correlationId);
        updatedSaga.CurrentState.Should().Be(TransferSagaStatus.Failed);
    }

    public void Dispose() => _db.Dispose();
}
