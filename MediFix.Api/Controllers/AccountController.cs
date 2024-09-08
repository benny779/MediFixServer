using MediatR;
using MediFix.Application.Users.CreateUser;
using MediFix.Application.Users.Login;
using MediFix.Application.Users.Logout;
using MediFix.Application.Users.RefreshToken;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class AccountController(ISender sender) : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        return result.Match(Created, Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var request = new LogoutRequest();

        var result = await sender.Send(request, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        return result.Match(Ok, Problem);
    }
}
