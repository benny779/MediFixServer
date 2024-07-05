using MediatR;
using MediFix.Application.Utils.Persistence.ResetDb;
using MediFix.Application.Utils.Persistence.Seed;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class UtilsController(ISender sender) : ApiController
{
    [HttpPost("persistence/reset")]
    public async Task<IActionResult> Reset(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ResetDbCommand(), cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpPost("persistence/seed")]
    public async Task<IActionResult> Seed(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SeedDataCommand(), cancellationToken);

        return result.Match(Ok, Problem);
    }
}