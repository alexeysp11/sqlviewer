namespace SqlViewer.Shared.Seed.Models;

public record UserSeedDto(
    Guid Uid,
    string Username,
    string Role
);
