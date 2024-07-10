using MediatR;
using MediFix.Application.ServiceCalls.AssignPractitioner;
using MediFix.Application.ServiceCalls.CreateServiceCall;
using MediFix.Application.ServiceCalls.GetServiceCall;
using MediFix.Application.ServiceCalls.GetServiceCallsWithFilter;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

[Authorize]
public class ServiceCallsController(ISender sender) : ApiController
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetServiceCallRequest(id);
        
        var result = await sender.Send(request, cancellationToken);

        return result.Match(Ok, Problem);
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateServiceCallCommand request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        return result.Match(value =>
            CreatedAtAction(nameof(GetById), value),
            Problem);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetServiceCallsWithFilterRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpPatch("{id:guid}/assign/{practitionerId:guid}")]
    public async Task<IActionResult> AssignPractitioner(
        Guid id,
        Guid practitionerId,
        CancellationToken cancellationToken)
    {
        var command = new AssignPractitionerCommand(id, practitionerId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(Ok, Problem);
    }
}
