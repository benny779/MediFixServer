using MediFix.Application.Abstractions.Services;
using MediFix.Application.Users.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MediFix.Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateAccessToken(ApplicationUser user)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub , user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.Name, user.FullName),
            new(ClaimTypes.Name, user.FullName),
        };

        var signingCredentials = new SigningCredentials(
            _jwtOptions.GetSigningKey(),
            SecurityAlgorithm);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.Now.AddHours(1),
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return tokenValue;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[256];
        using var numberGenerator = RandomNumberGenerator.Create();
        numberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _jwtOptions.GetSigningKey(),
            ClockSkew = TimeSpan.Zero
        };

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                tokenValidationParameters,
                out SecurityToken securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken is null
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithm,
                StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal;
    }
}
