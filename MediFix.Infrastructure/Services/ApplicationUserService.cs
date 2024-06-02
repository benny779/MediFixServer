using MediFix.Application.Abstractions.Data;
using MediFix.Application.Users;
using MediFix.Application.Users.Entities;
using Microsoft.AspNetCore.Identity;

namespace MediFix.Infrastructure.Services;

internal class ApplicationUserService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IApplicationUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : IApplicationUserService
{
    private const bool LockoutOnFailure = false;
    private const int RefreshTokenValidityDays = 4;

    public Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return userManager.FindByEmailAsync(email);
    }

    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        return userManager.CreateAsync(user, password);
    }

    public async Task<bool> SetRefreshTokenAsync(ApplicationUser user, string refreshToken)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenValidity = DateTime.Now.AddDays(RefreshTokenValidityDays);

        userRepository.Update(user);

        return await unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsRefreshTokenValidAsync(string email, string refreshToken)
    {
        var user = await userManager.FindByEmailAsync(email);

        return user is not null && user.IsRefreshTokenValid(refreshToken);
    }


    public Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password)
    {
        return signInManager.CheckPasswordSignInAsync(user, password, LockoutOnFailure);
    }
}
