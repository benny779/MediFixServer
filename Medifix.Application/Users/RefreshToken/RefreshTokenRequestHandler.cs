using MediatR;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.Application.Users.Entities;
using MediFix.Application.Users.GenerateAndUpdateTokens;
using MediFix.SharedKernel.Results;
using System.Security.Claims;

namespace MediFix.Application.Users.RefreshToken;

internal sealed class RefreshTokenRequestHandler(
    IJwtProvider jwtProvider,
    IApplicationUserService applicationUserService,
    ISender sender)
    : ICommandHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        Claim? emailClaim = jwtProvider.GetEmailClaim(request.AccessToken);
        if (emailClaim is null)
        {
            return Error.Unauthorized();
        }

        var user = await applicationUserService.FindByEmailAsync(emailClaim.Value);
        if (user is null)
        {
            return Error.Unauthorized();
        }

        if (!user.IsRefreshTokenValid(request.RefreshToken))
        {
            return Error.Unauthorized();
        }

        var tokensRequest = new GenerateAndUpdateTokensCommand(user);
        var tokensResult = await sender.Send(tokensRequest, cancellationToken);
        if (tokensResult.IsFailure)
        {
            return tokensResult.Error;
        }

        (string accessToken, string refreshToken) = tokensResult.Value;

        return new RefreshTokenResponse(accessToken, refreshToken);
    }
}