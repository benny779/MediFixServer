using MediatR;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Users.GenerateAndUpdateTokens;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users.Login;

internal sealed class LoginRequestHandler(
    IApplicationUserService applicationUserService,
    ISender sender)
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

        var tokensRequest = new GenerateAndUpdateTokensCommand(user);
        var tokensResult = await sender.Send(tokensRequest, cancellationToken);
        if (tokensResult.IsFailure)
        {
            return tokensResult.Error;
        }

        (string accessToken, string refreshToken) = tokensResult.Value!;

        return new LoginResponse(
            accessToken,
            refreshToken);
    }
}