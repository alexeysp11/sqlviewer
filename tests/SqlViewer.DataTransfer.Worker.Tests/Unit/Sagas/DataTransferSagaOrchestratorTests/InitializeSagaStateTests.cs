using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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

public sealed class InitializeSagaStateTests : IDisposable
{
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock;
    private readonly Mock<IServiceScope> _scopeMock;
    private readonly Mock<ILogger<DataTransferSagaOrchestrator>> _loggerMock;
    private readonly DataTransferDbContext _dbContext;

    private readonly Mock<AccessabilityCheckStep> _accessStepMock;
    private readonly Mock<SchemaValidationStep> _schemaStepMock;
    private readonly Mock<DataTransferStep> _transferStepMock;
    private readonly Mock<CompensationStep> _compensationStepMock;

    private readonly DataTransferSagaOrchestrator _orchestrator;
    private readonly Fixture _fixture = new();

    public InitializeSagaStateTests()
    {
        // Setting up an In-Memory database
        DbContextOptions<DataTransferDbContext> options = new DbContextOptionsBuilder<DataTransferDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new DataTransferDbContext(options);

        // Setting up DI mocks
        _scopeFactoryMock = new Mock<IServiceScopeFactory>();
        _scopeMock = new Mock<IServiceScope>();
        Mock<IServiceProvider> serviceProviderMock = new();

        _scopeFactoryMock.Setup(x => x.CreateScope()).Returns(_scopeMock.Object);
        _scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(DataTransferDbContext))).Returns(_dbContext);

        // Mocks.
        _loggerMock = new Mock<ILogger<DataTransferSagaOrchestrator>>();
        _accessStepMock = new Mock<AccessabilityCheckStep>(new Mock<ILogger<AccessabilityCheckStep>>().Object);
        _schemaStepMock = new Mock<SchemaValidationStep>(new Mock<ILogger<SchemaValidationStep>>().Object);
        _transferStepMock = new Mock<DataTransferStep>(new Mock<ILogger<DataTransferStep>>().Object, new Mock<IConnectionMultiplexer>().Object);
        _compensationStepMock = new Mock<CompensationStep>(new Mock<ILogger<CompensationStep>>().Object);

        // Create an orchestrator (steps can be mocked, as they are not involved in this method)
        _orchestrator = new DataTransferSagaOrchestrator(
            scopeFactory: _scopeFactoryMock.Object,
            logger: _loggerMock.Object,
            accessStep: _accessStepMock.Object,
            schemaStep: _schemaStepMock.Object,
            transferStep: _transferStepMock.Object,
            compensationStep: _compensationStepMock.Object);
    }

    [Fact]
    public async Task InitializeSagaStateAsync_ShouldCreateNewState_WhenInboxMessageExists()
    {
        // Arrange
        Guid correlationId = _fixture.Create<Guid>();
        string tableName = _fixture.Create<string>();
        StartDataTransferCommand command = _fixture.Build<StartDataTransferCommand>()
            .With(x => x.CorrelationId, correlationId)
            .With(x => x.TableName, tableName)
            .Create();
        InboxMessageEntity inboxMessage = _fixture.Build<InboxMessageEntity>()
            .With(x => x.CorrelationId, correlationId)
            .With(x => x.Payload, JsonSerializer.Serialize(command))
            .Create();
        _dbContext.InboxMessages.Add(inboxMessage);
        await _dbContext.SaveChangesAsync();

        // Act
        await _orchestrator.InitializeSagaStateAsync(correlationId, CancellationToken.None);

        // Assert
        DataTransferSagaEntity? saga = await _dbContext.DataTransferSagas.FirstOrDefaultAsync(x => x.CorrelationId == correlationId);
        saga.Should().NotBeNull();
        saga.TableName.Should().Be(tableName);
        saga.CurrentState.Should().Be(TransferSagaStatus.Initial);
    }

    [Fact]
    public async Task InitializeSagaStateAsync_ShouldThrowException_WhenInboxMessageMissing()
    {
        // Arrange
        Guid correlationId = _fixture.Create<Guid>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _orchestrator.InitializeSagaStateAsync(correlationId, CancellationToken.None));
    }

    [Fact]
    public async Task InitializeSagaStateAsync_ShouldNotDuplicate_WhenStateAlreadyExists()
    {
        // Arrange
        Guid correlationId = _fixture.Create<Guid>();
        string oldTableName = _fixture.Create<string>();
        StartDataTransferCommand command = _fixture.Build<StartDataTransferCommand>()
            .With(x => x.CorrelationId, correlationId)
            .Create();
        InboxMessageEntity inboxMessage = _fixture.Build<InboxMessageEntity>()
            .With(x => x.CorrelationId, correlationId)
            .With(x => x.Payload, JsonSerializer.Serialize(command))
            .Create();
        DataTransferSagaEntity saga = _fixture.Build<DataTransferSagaEntity>()
            .With(x => x.CorrelationId, correlationId)
            .With(x => x.TableName, oldTableName)
            .Create();
        _dbContext.InboxMessages.Add(inboxMessage);
        _dbContext.DataTransferSagas.Add(saga);
        await _dbContext.SaveChangesAsync();

        // Act
        await _orchestrator.InitializeSagaStateAsync(correlationId, CancellationToken.None);

        // Assert
        List<DataTransferSagaEntity> sagas = await _dbContext.DataTransferSagas
            .Where(x => x.CorrelationId == correlationId)
            .ToListAsync();
        sagas.Should().HaveCount(1, because: "There should be only one entry left");
        sagas[0].TableName.Should().Be(oldTableName, because: "The new entry should not overwrite the old one in this method.");
    }

    public void Dispose() => _dbContext.Dispose();
}
