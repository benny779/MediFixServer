using MediFix.Application.Users.Entities;
using System.Security.Claims;

namespace MediFix.Application.Abstractions.Services;

public interface IJwtProvider
{
    string GenerateAccessToken(ApplicationUser user);
    string GenerateRefreshToken();
    Claim? GetEmailClaim(string token);
}