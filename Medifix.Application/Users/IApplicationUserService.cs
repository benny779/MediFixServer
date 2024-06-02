using MediFix.Application.Users.Entities;
using Microsoft.AspNetCore.Identity;

namespace MediFix.Application.Users;

public interface IApplicationUserService
{
    Task<ApplicationUser?> FindByEmailAsync(string email);

    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

    Task<bool> SetRefreshTokenAsync(ApplicationUser user, string refreshToken);

    Task<bool> IsRefreshTokenValidAsync(string email, string refreshToken);

    Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password);
}
