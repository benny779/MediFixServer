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
        IList<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.Name, user.FullName),
            new("roles", user.Type.ToString())
        ];

        if (user.PhoneNumber is not null)
        {
            claims.Add(new("phone", user.PhoneNumber));
        }

        var signingCredentials = new SigningCredentials(
            _jwtOptions.GetSigningKey(),
            SecurityAlgorithm);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.Now.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes),
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

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = _jwtOptions.GetSigningKey(),
            ClockSkew = TimeSpan.Zero
        };

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                tokenValidationParameters,
                out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithm,
                StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        return principal;
    }

    private IEnumerable<Claim> GetClaimsFromExpiredToken(string token)
    {
        var principal = GetPrincipalFromExpiredToken(token);

        return principal?.Claims ?? Enumerable.Empty<Claim>();
    }

    private static Claim? FindClaim(IEnumerable<Claim> claims, string claimType)
    {
        var claimsList = claims.ToList();

        return claimsList.SingleOrDefault(c => c.Type.Equals(claimType))
               ?? claimsList.SingleOrDefault(c => c.Properties.Any(p => p.Value.Equals(claimType)));
    }

    public Claim? GetEmailClaim(string token)
    {
        var claims = GetClaimsFromExpiredToken(token);

        return FindClaim(claims, JwtRegisteredClaimNames.Email);
    }
}
