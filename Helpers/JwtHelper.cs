using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Transferciniz.API.DTOs;
using Transferciniz.API.Entities;

namespace Transferciniz.API.Helpers;

public static class JwtHelper
{
    public static string GenerateToken(this Account account, IConfiguration _configuration, Guid sessionId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, account.Email),
            new Claim(JwtRegisteredClaimNames.Name, account.Name),
            new Claim("surname", account.Surname),
            new Claim("id", account.Id.ToString()),
            new Claim("sessionId", sessionId.ToString()),
            new Claim("profilePicture", account.ProfilePicture),
            new Claim("accountType", account.AccountType.ToString()),
            new Claim("username", account.Username),
            new Claim("isAccountCompleted", account.IsAccountCompleted.ToString()),
            new Claim("latitude", account.Latitude.ToString(CultureInfo.InvariantCulture)),
            new Claim("longitude",account.Longitude.ToString(CultureInfo.InvariantCulture))
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
    
    public static AccountDto ToDto(this Account account)
    {
        var defaultAddress = account.AccountLocations.First(x => x.IsDefault);
        return new AccountDto
        {
            Id = account.Id,
            AccountType = account.AccountType,
            ProfilePicture = account.ProfilePicture,
            Username = account.Username,
            Name = account.Name,
            Surname = account.Surname,
            Latitude = defaultAddress.Latitude,
            Longitude = defaultAddress.Longitude
        };
    }
    
}