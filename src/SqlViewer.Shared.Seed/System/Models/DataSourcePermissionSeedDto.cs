namespace SqlViewer.Shared.Seed.System.Models;

public sealed record DataSourcePermissionSeedDto(
    Guid Uid,
    Guid UserUid,
    Guid DataSourceUid,
    string AccessLevel);
