using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetNova.API.Shared.Application.Services;
using PetNova.API.Veterinary.IAM.Application.Services;
using PetNova.API.Veterinary.IAM.Interface.DTOs;

namespace PetNova.API.Veterinary.IAM.Interface.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService   _auth;
    private readonly ITokenService _tokens;

    public AuthController(AuthService auth, ITokenService tokens)
    {
        _auth   = auth;
        _tokens = tokens;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var user = await _auth.RegisterAsync(dto);
        if (user is null) return BadRequest(new { message = "Username or email already exists." });

        var token = _tokens.GenerateToken(user);
        return Ok(new { user, token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var user = await _auth.AuthenticateAsync(dto);
        if (user is null) return Unauthorized(new { message = "Invalid credentials." });

        var token = _tokens.GenerateToken(user);
        return Ok(new { user, token });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> Users() => Ok(await _auth.ListUsersAsync());

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id is null) return Unauthorized();

        var user = await _auth.GetUserByIdAsync(Guid.Parse(id));
        return user is null ? NotFound() : Ok(user);
    }
}
