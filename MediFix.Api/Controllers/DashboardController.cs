using MediatR;
using MediFix.Application.Dashboard;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

[Authorize]
public class DashboardController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetDashboardRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        return result.Match(Ok, Problem);
    }
}
