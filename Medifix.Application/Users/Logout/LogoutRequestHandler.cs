using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.Application.Extensions;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users.Logout;

internal sealed class LogoutRequestHandler(
    ICurrentUser currentUser,
    IApplicationUserService applicationUserService)
    : ICommandHandler<LogoutRequest>
{
    public async Task<Result> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        var user = await applicationUserService.FindByEmailAsync(currentUser.Email);

        if (user is null)
        {
            return Error.Unauthorized();
        }

        var logoutResult = await applicationUserService.RevokeRefreshTokenAsync(user);

        return logoutResult.ToResult();
    }
}