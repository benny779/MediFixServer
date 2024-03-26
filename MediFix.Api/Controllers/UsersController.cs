using MediatR;
using MediFix.Application.Users.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var createResult = await sender.Send(request, cancellationToken);

        if (createResult.IsFailure)
            return BadRequest(createResult.Error);

        return Ok();
    }
}