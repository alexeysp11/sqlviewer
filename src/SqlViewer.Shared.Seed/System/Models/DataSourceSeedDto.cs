namespace SqlViewer.Shared.Seed.System.Models;

public sealed record DataSourceSeedDto(
    Guid Uid,
    string Name,
    string Description,
    string DbType,
    Guid OwnerUid
);
