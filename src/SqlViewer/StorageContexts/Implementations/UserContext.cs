using SqlViewer.Shared.Dtos.Auth;
using SqlViewer.StorageContexts.Abstractions;

namespace SqlViewer.StorageContexts.Implementations;

#nullable enable

public sealed record UserContext : IUserContext
{
    public LoginResponseDto? CurrentUser { get; set; }
}
