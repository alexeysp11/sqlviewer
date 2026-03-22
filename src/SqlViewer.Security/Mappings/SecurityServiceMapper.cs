using Riok.Mapperly.Abstractions;
using SqlViewer.Shared.Dtos.Auth;

namespace SqlViewer.Security.Mappings;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0051:Remove unused private members", Justification = "<Pending>")]
public partial class SecurityServiceMapper
{
    [MapProperty(nameof(LoginResponseDto.RefreshToken), nameof(LoginResponse.RefreshToken), Use = nameof(MapString))]
    [MapProperty(nameof(LoginResponseDto.Username), nameof(LoginResponse.Username), Use = nameof(MapString))]
    [MapProperty(nameof(LoginResponseDto.UserUid), nameof(LoginResponse.UserUid), Use = nameof(MapUserUid))]
    public partial LoginResponse MapToGrpc(LoginResponseDto dto);

    private static string MapString(string? value) => value ?? string.Empty;
    private static string MapUserUid(Guid? value) => value?.ToString() ?? string.Empty;
}