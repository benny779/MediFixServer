using MediatR;
using MediFix.Application.Users.CreateUser;
using MediFix.Application.Users.Login;
using MediFix.SharedKernel.Results;
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
}
