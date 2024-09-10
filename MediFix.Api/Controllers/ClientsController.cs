using MediatR;
using MediFix.Application.Users.Clients.GetClients;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

[Authorize]
public class ClientsController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var query = new GetClientsRequest();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }
}
