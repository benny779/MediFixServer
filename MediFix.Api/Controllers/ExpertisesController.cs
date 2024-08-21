using MediatR;
using MediFix.Application.Expertises.CreateExpertise;
using MediFix.Application.Expertises.GetExpertise;
using MediFix.Application.Expertises.GetExpertises;
using MediFix.Application.Expertises.UpdateExpertise;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class ExpertisesController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var query = new GetExpertisesRequest();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetExpertiseRequest(id);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExpertiseCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.Match(value =>
            CreatedAtAction(nameof(GetById), value),
            Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateExpertiseCommand request, CancellationToken cancellationToken)
    {
        if (IsIdsMismatch(id, request.ExpertiseId, out var problem))
        {
            return problem!;
        }

        var result = await sender.Send(request, cancellationToken);

        return result.Match(NoContent, Problem);
    }
}