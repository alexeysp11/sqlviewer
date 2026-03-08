namespace SqlViewer.Shared.Seed.Models;

public sealed record DataSourcePermissionSeedDto(
    Guid Uid,
    Guid UserUid,
    Guid DataSourceUid,
    string AccessLevel);
