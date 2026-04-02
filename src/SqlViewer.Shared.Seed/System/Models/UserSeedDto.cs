namespace SqlViewer.Shared.Seed.System.Models;

public sealed record UserSeedDto(
    Guid Uid,
    string Username,
    string Role
);
