using MediatR;
using MediFix.Application.Locations.CreateLocation;
using MediFix.Application.Locations.DeleteLocation;
using MediFix.Application.Locations.GetLocation;
using MediFix.Application.Locations.GetLocationChildren;
using MediFix.Application.Locations.SetActiveStatus;
using MediFix.Domain.Locations;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] bool includeParents, CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
        var query = new GetLocationRequest(locationId, includeParents);

        var locationResult = await sender.Send(query, cancellationToken);

        return locationResult.IsFailure
            ? NotFound(locationResult.Error)
            : Ok(locationResult.Value);
    }

    [HttpGet("{id:guid}/children")]
    public async Task<IActionResult> GetChildren(Guid id, CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
        var query = new GetLocationChildrenRequest(locationId);

        var locationsResult = await sender.Send(query, cancellationToken);

        return locationsResult.IsFailure
            ? NotFound(locationsResult.Error)
            : Ok(locationsResult.Value);
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var createResult = await sender.Send(request, cancellationToken);

        return createResult.IsFailure
            ? BadRequest(createResult.Error)
            : CreatedAtAction(
                nameof(GetById),
                createResult.Value!);
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        return await SetActiveMode(id, true, cancellationToken);
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        return await SetActiveMode(id, false, cancellationToken);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
        var request = new DeleteLocationCommand(locationId);

        var deleteResult = await sender.Send(request, cancellationToken);

        return deleteResult.IsSuccess ? NoContent() : Problem(deleteResult.Error.Description);
    }


    public override CreatedAtActionResult CreatedAtAction(string? actionName, object? value)
    {
        if (!HasId(value, out var id))
        {
            return base.CreatedAtAction(actionName, value);
        }

        return CreatedAtAction(
            actionName,
            new { id },
            value);
    }

    private static bool HasId(object? value, out object? id)
    {
        var property = value?.GetType().GetProperty("Id");

        id = property?.GetValue(value);

        return id is not null;
    }


    private async Task<IActionResult> SetActiveMode(Guid id, bool isActive, CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
        var command = new SetLocationActiveStatusCommand(locationId, isActive);

        var updateResult = await sender.Send(command, cancellationToken);

        return updateResult.IsSuccess ? NoContent() : Problem(updateResult.Error.Description);
    }
}