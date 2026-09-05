using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace auth.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        var secretKey = _configuration["JwtSettings:SecretKey"];
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

    }

    public string GenerateToken(User user)
    {
        var claim = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name,(user.UserName ?? "Unknown")),
            new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claim),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpireInMinutes"])),
            SigningCredentials = new SigningCredentials(_key,SecurityAlgorithms.HmacSha256)

        };

        var tokenHandler= new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}