namespace SqlViewer.Shared.Seed.Models;

public sealed record DataSourcePermissionSeedDto(
    Guid UserUid,
    Guid DataSourceUid,
    string AccessLevel);
