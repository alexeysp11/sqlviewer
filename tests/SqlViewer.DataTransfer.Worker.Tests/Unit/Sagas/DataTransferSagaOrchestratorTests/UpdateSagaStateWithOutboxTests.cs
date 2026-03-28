using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using SqlViewer.DataTransfer.Worker.Data.DbContexts;
using SqlViewer.DataTransfer.Worker.Data.Entities;
using SqlViewer.DataTransfer.Worker.Sagas.SagaSteps;
using SqlViewer.DataTransfer.Worker.Sagas;
using SqlViewer.Etl.Core.Enums;
using SqlViewer.Shared.Messages.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using SqlViewer.Shared.Messages.Etl.Events;

namespace SqlViewer.DataTransfer.Worker.Tests.Unit.Sagas.DataTransferSagaOrchestratorTests;

public sealed class UpdateSagaStateWithOutboxTests : IDisposable
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

    public UpdateSagaStateWithOutboxTests()
    {
        // In-Memory database.
        DbContextOptions<DataTransferDbContext> options = new DbContextOptionsBuilder<DataTransferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _db = new DataTransferDbContext(options);

        _configMock.Setup(c => c[It.IsAny<string>()]).Returns(_fixture.Create<string>());

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
        serviceProviderMock.Setup(x => x.GetService(typeof(IConfiguration))).Returns(_configMock.Object);

        _orchestrator = new DataTransferSagaOrchestrator(
            _scopeFactoryMock.Object,
            _loggerMock.Object,
            _accessStepMock.Object,
            _schemaStepMock.Object,
            _transferStepMock.Object,
            _compensationStepMock.Object);
    }

    [Fact]
    public async Task UpdateSagaStateWithOutboxAsync_ShouldUpdateDbAndAddOutboxMessage()
    {
        // Arrange
        Guid correlationId = _fixture.Create<Guid>();
        string userUid = _fixture.Create<string>();

        DataTransferSagaEntity saga = _fixture.Build<DataTransferSagaEntity>()
            .With(x => x.CorrelationId, correlationId)
            .With(x => x.CurrentState, TransferSagaStatus.Initial)
            .With(x => x.UserUid, userUid)
            .Create();
        _db.DataTransferSagas.Add(saga);
        await _db.SaveChangesAsync();

        DataTransferStatusUpdated expectedEvent = new(
            CorrelationId: correlationId,
            TransferStatus: TransferStatus.Progress.ToString(),
            InternalStatus: TransferSagaStatus.Transferring.ToString(),
            Timestamp: DateTime.UtcNow,
            ErrorMessage: "Error"
        );

        // Act
        await _orchestrator.UpdateSagaStateWithOutboxAsync(
            correlationId: correlationId,
            status: TransferSagaStatus.Transferring,
            error: "Error",
            ct: CancellationToken.None);

        // Assert
        DataTransferSagaEntity updatedSaga = await _db.DataTransferSagas.FirstAsync(x => x.CorrelationId == correlationId);
        updatedSaga.CurrentState.Should().Be(TransferSagaStatus.Transferring);

        OutboxMessageEntity? outboxMessage = await _db.OutboxMessages.FirstOrDefaultAsync(x => x.CorrelationId == correlationId);
        outboxMessage.Should().NotBeNull();
        outboxMessage.CorrelationId.Should().Be(correlationId);
        outboxMessage.MessageType.Should().Be(nameof(DataTransferStatusUpdated));
        outboxMessage.UserUid.Should().Be(userUid);

        DataTransferStatusUpdated transferCommand = JsonSerializer.Deserialize<DataTransferStatusUpdated>(outboxMessage.Payload)!;
        transferCommand.Should().BeEquivalentTo(expectedEvent, options => options
            .Excluding(x => x.Timestamp)    // Exclude time, since it is generated inside the method.
            .WithStrictOrdering());
    }

    public void Dispose() => _db.Dispose();
}
