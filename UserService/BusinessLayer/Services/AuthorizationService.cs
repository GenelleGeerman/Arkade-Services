using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        var claims = new[]
        {
            new Claim("id", userInfo.Id.ToString()),
            new Claim("email", userInfo.Email),
            new Claim("firstName", userInfo.FirstName),
            new Claim("lastName", userInfo.LastName)
            // Add additional claims as needed
        };
        JwtSecurityToken token = new(config["Jwt:Issuer"],
            config["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(TOKEN_EXPIRATION_IN_MINUTES),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public long GetId(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(config["Jwt:Key"]!);

        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // You can adjust this value
        };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out _);

            foreach (Claim c in principal.Claims)
            {
                Console.WriteLine(c.Type+": " + c.Value);
                Console.WriteLine(c.ValueType+": " + c.Value);
            }
            string? value = principal.Claims.FirstOrDefault(c => c.Type == "id").Value;
        try
        {
            return value != null ? long.Parse(value) : throw new NullReferenceException("AUTH ISSUE");
        }
        catch
        {
            throw new UnauthorizedAccessException(value);
        }
    }
}
