using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Helpers;

public static class JwtHelper
{
    public static string GenerateToken(this User user, IConfiguration _configuration, Guid sessionId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim("surname", user.Surname),
            new Claim("id", user.Id.ToString()),
            new Claim("sessionId", sessionId.ToString()),
            new Claim("companyId", user.CompanyId.ToString() ?? string.Empty),
            new Claim("userType", user.UserType.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}