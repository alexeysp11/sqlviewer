using SqlViewer.Etl.Core.Enums;

namespace SqlViewer.Etl.Core.Data.Projections;

public sealed record TransferJobDbProjection(
    Guid CorrelationId,
    TransferStatus CurrentStatus,
    bool IsFinalState)
{
    public string StatusMessage => CurrentStatus.ToString();
}