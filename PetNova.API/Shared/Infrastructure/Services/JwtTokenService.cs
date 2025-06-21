using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PetNova.API.Shared.Application.Services;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;

namespace PetNova.API.Shared.Infrastructure.Services;

public class JwtTokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(User user)
    {
        var key      = configuration["Jwt:Key"     ] ?? throw new("Jwt:Key missing");
        var issuer   = configuration["Jwt:Issuer"  ] ?? throw new("Jwt:Issuer missing");
        var audience = configuration["Jwt:Audience"] ?? throw new("Jwt:Audience missing");

        var securityKey  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials  = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name         , user.Username),
            new Claim(ClaimTypes.Email        , user.Email),
            new Claim(ClaimTypes.Role         , user.Role)
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
