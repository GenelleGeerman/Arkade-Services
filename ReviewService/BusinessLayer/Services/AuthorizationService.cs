using System.Text;
using BusinessLayer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLayer.Services;

public class AuthorizationService(IConfiguration config) : IAuthorizationService
{
    public int GetUserId(string token)
    {
        ClaimsPrincipal principal = GetPrincipal(token);
        string? value = principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        try
        {
            return value != null ? int.Parse(value) : throw new NullReferenceException("AUTH ISSUE");
        }
        catch
        {
            throw new UnauthorizedAccessException(value);
        }
    }

    private ClaimsPrincipal GetPrincipal(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(config["JwtKey"]!);

        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = config["JwtIssuer"],
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // You can adjust this value
        };

        return tokenHandler.ValidateToken(token, validationParameters, out _);
    }
}
