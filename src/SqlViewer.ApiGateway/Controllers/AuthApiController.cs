using Microsoft.AspNetCore.Mvc;
using SqlViewer.ApiGateway.Services;
using SqlViewer.Common.Constants;
using SqlViewer.Common.Dtos;
using SqlViewer.Common.Dtos.Auth;
using SqlViewer.Common.Enums;

namespace SqlViewer.ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AuthApiController(ILogger<MetadataApiController> logger, IAuthService authService) : ControllerBase
{
    private readonly ILogger<MetadataApiController> _logger = logger;
    private readonly IAuthService _authService = authService;

    [HttpPost]
    [Route(RestApiPaths.Auth.Login)]
    public async Task<CommonResponseDto> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            bool success = request.AuthType switch
            {
                SqlViewerAuthType.ByPassword => await _authService.VilidateByPasswordAsync(request.Username, request.Password),
                _ => throw new NotSupportedException($"Specified authentication type is not supported: {request.AuthType}")
            };
            if (!success)
            {
                throw new InvalidOperationException("Authentication failed");
            }
            return new() { Status = SqlOperationStatus.Success, };
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            return new() { Status = SqlOperationStatus.Failed, ErrorMessage = ex.Message, };
        }
    }
}
