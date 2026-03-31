using FluentAssertions;
using SqlViewer.Etl.Core.Enums;

namespace SqlViewer.Etl.Core.Tests.Enums;

public sealed class TransferStatusExtensionsTests
{
    [Theory]
    [InlineData(TransferSagaStatus.Initial, TransferStatus.Queued)]
    [InlineData(TransferSagaStatus.AccessabilityCheck, TransferStatus.Started)]
    [InlineData(TransferSagaStatus.SchemaValidation, TransferStatus.Progress)]
    [InlineData(TransferSagaStatus.Transferring, TransferStatus.Progress)]
    [InlineData(TransferSagaStatus.Completed, TransferStatus.Completed)]
    [InlineData(TransferSagaStatus.Failed, TransferStatus.Failed)]
    [InlineData(TransferSagaStatus.TimedOut, TransferStatus.Failed)]
    [InlineData(TransferSagaStatus.Cancelled, TransferStatus.Failed)]
    public void ToTransferStatus_ShouldMapSagaStatusToCorrectClientStatus(
        TransferSagaStatus sagaStatus,
        TransferStatus expectedClientStatus)
    {
        // Act
        TransferStatus result = sagaStatus.ToTransferStatus();

        // Assert
        result.Should().Be(expectedClientStatus);
    }

    [Fact]
    public void ToTransferStatus_ShouldReturnNone_WhenSagaStatusIsUndefined()
    {
        // Arrange
        TransferSagaStatus undefinedStatus = (TransferSagaStatus)999;

        // Act
        TransferStatus result = undefinedStatus.ToTransferStatus();

        // Assert
        result.Should().Be(TransferStatus.None);
    }

    [Theory]
    [InlineData(TransferStatus.Queued, TransferSagaStatus.Initial)]
    [InlineData(TransferStatus.Started, TransferSagaStatus.AccessabilityCheck)]
    [InlineData(TransferStatus.Progress, TransferSagaStatus.Transferring)]
    [InlineData(TransferStatus.Completed, TransferSagaStatus.Completed)]
    [InlineData(TransferStatus.Failed, TransferSagaStatus.Failed)]
    public void ToSagaStatus_ShouldMapClientStatusToPrimarySagaStatus(
        TransferStatus clientStatus,
        TransferSagaStatus expectedSagaStatus)
    {
        // Act
        TransferSagaStatus result = clientStatus.ToSagaStatus();

        // Assert
        result.Should().Be(expectedSagaStatus);
    }

    [Fact]
    public void ToTransferStatus_ShouldMapAllFailedStatesToSingleFailedClientStatus()
    {
        // Arrange
        TransferSagaStatus[] failedStates =
        [
            TransferSagaStatus.Failed,
            TransferSagaStatus.TimedOut,
            TransferSagaStatus.Cancelled
        ];

        // Act & Assert
        foreach (TransferSagaStatus state in failedStates)
        {
            state.ToTransferStatus().Should().Be(TransferStatus.Failed,
                $"because saga state {state} should be perceived as Failure by the user");
        }
    }

    [Theory]
    [InlineData(TransferStatus.None, 0)]
    [InlineData(TransferStatus.Queued, 1)]
    [InlineData(TransferStatus.Started, 2)]
    [InlineData(TransferStatus.Progress, 3)]
    [InlineData(TransferStatus.Completed, 4)]
    [InlineData(TransferStatus.Failed, 5)]
    [InlineData(TransferStatus.Cancelled, 5)]
    [InlineData(TransferStatus.TimedOut, 5)]
    public void GetStatusRank_ShouldReturnCorrectPriority(TransferStatus status, int expectedRank)
    {
        // Act
        int actualRank = TransferStatusExtensions.GetStatusRank(status);

        // Assert
        actualRank.Should().Be(expectedRank, because: $"Status {status} must have rank {expectedRank}");
    }

    [Fact]
    public void GetStatusRank_ShouldEnsureProgressIsLowerThanCompleted()
    {
        // Arrange & Act
        int progressRank = TransferStatusExtensions.GetStatusRank(TransferStatus.Progress);
        int completedRank = TransferStatusExtensions.GetStatusRank(TransferStatus.Completed);

        // Assert
        progressRank.Should().BeLessThan(completedRank);
    }
}
