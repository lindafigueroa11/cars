using Backend.DTOs.Auth;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> LoginGoogle(LoginGoogleDTO dto)
    {
        var result = await _authService.LoginWithGoogle(dto);
        return Ok(result);
    }
}
