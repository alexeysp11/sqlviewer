using Riok.Mapperly.Abstractions;
using SqlViewer.Security;
using SqlViewer.Shared.Dtos.Auth;
using SqlViewer.Shared.Enums;

namespace SqlViewer.ApiGateway.Mappings;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0051:Remove unused private members", Justification = "<Pending>")]
public partial class LoginMapper
{
    [MapperIgnoreSource(nameof(LoginResponse.HasUsername))]
    [MapperIgnoreSource(nameof(LoginResponse.HasRefreshToken))]
    [MapperIgnoreSource(nameof(LoginResponse.HasUserUid))]
    [MapProperty(nameof(LoginResponse.UserUid), nameof(LoginResponseDto.UserUid), Use = nameof(MapUserUid))]
    public partial LoginResponseDto MapToDto(LoginResponse response);

    [MapProperty(nameof(LoginRequestDto.AuthType), nameof(LoginRequest.AuthType), Use = nameof(MapToAuthType))]
    public partial LoginRequest MapToLoginRequest(LoginRequestDto request);

    private static AuthType MapToAuthType(SqlViewerAuthType authType) => authType switch
    {
        SqlViewerAuthType.ByPassword => AuthType.Password,
        _ => AuthType.Unknown,
    };

    private static Guid? MapUserUid(string uid) => !string.IsNullOrEmpty(uid) ? Guid.Parse(uid) : null;

    private static string MapString(string? value) => value ?? string.Empty;
}
