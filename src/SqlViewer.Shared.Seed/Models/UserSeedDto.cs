namespace SqlViewer.Shared.Seed.Models;

public sealed record UserSeedDto(
    Guid Uid,
    string Username,
    string Role
);
