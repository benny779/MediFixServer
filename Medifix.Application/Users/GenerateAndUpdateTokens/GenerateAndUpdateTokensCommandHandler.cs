using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.Application.Extensions;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users.GenerateAndUpdateTokens;

internal sealed class GenerateAndUpdateTokensCommandHandler(
    IJwtProvider jwtProvider,
    IApplicationUserService applicationUserService)
    : ICommandHandler<GenerateAndUpdateTokensCommand, TokensResponse>
{
    public async Task<Result<TokensResponse>> Handle(GenerateAndUpdateTokensCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;

        string accessToken = jwtProvider.GenerateAccessToken(user);
        string refreshToken = jwtProvider.GenerateRefreshToken();

        var refreshTokenUpdateResult = await applicationUserService.SetRefreshTokenAsync(user, refreshToken);

        if (!refreshTokenUpdateResult.Succeeded)
        {
            return refreshTokenUpdateResult.Errors.ToValidationError();
        }

        return new TokensResponse(accessToken, refreshToken);
    }
}