using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users.Login;

internal sealed class LoginRequestHandler(
    IApplicationUserService applicationUserService,
    IJwtProvider jwtProvider)
    : ICommandHandler<LoginRequest, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await applicationUserService.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Error.Unauthorized();
        }

        var result = await applicationUserService
            .CheckPasswordSignInAsync(user, request.Password);

        if (result.IsLockedOut)
        {
            return UserErrors.LockedOut(request.Email);
        }

        if (!result.Succeeded)
        {
            return Error.Unauthorized();
        }

        string accessToken = jwtProvider.GenerateAccessToken(user);
        string refreshToken = jwtProvider.GenerateRefreshToken();

        bool refreshTokenUpdated = await applicationUserService.SetRefreshTokenAsync(user, refreshToken);

        return new LoginResponse(
            accessToken,
            refreshTokenUpdated ? refreshToken : null);
    }
}