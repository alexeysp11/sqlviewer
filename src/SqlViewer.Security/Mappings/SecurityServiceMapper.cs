using Riok.Mapperly.Abstractions;
using SqlViewer.Common.Dtos.Auth;

namespace SqlViewer.Security.Mappings;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0051:Remove unused private members", Justification = "<Pending>")]
public partial class SecurityServiceMapper
{
    public partial LoginResponse MapToGrpc(LoginResponseDto dto);
}