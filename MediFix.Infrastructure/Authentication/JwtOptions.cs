using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MediFix.Infrastructure.Authentication;

public class JwtOptions
{
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
}

public static class JwtOptionsExtensions
{
    public static SecurityKey GetSigningKey(this JwtOptions options)
        => new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(options.SecretKey));
}
