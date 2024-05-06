using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.Services;

public class AuthorizationService(IConfiguration config) : IAuthorizationService
{
    private const int TOKEN_EXPIRATION_IN_MINUTES = 120;

    public string GenerateToken(UserData userInfo)
    {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(config["Jwt:Issuer"],
            config["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(TOKEN_EXPIRATION_IN_MINUTES),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
