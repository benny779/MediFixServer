using MediFix.Application.Users;
using MediFix.Application.Users.Entities;
using MediFix.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace MediFix.Infrastructure.Services;

internal class ApplicationUserService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOptions<JwtOptions> jwtOptions)
    : IApplicationUserService
{
    private const bool LockoutOnFailure = false;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return userManager.FindByEmailAsync(email);
    }

    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        return userManager.CreateAsync(user, password);
    }

    public Task<IdentityResult> SetRefreshTokenAsync(ApplicationUser user, string refreshToken)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenValidity = DateTime.Now.AddDays(_jwtOptions.RefreshTokenExpiryDays);

        return userManager.UpdateAsync(user);
    }

    public Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password)
    {
        return signInManager.CheckPasswordSignInAsync(user, password, LockoutOnFailure);
    }
}
