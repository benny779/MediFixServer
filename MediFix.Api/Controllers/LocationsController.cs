using MediatR;
using MediFix.Application.Locations.ChangeLocationName;
using MediFix.Application.Locations.CreateLocation;
using MediFix.Application.Locations.DeleteLocation;
using MediFix.Application.Locations.GetLocation;
using MediFix.Application.Locations.GetLocationChildren;
using MediFix.Application.Locations.GetLocationsByType;
using MediFix.Application.Locations.GetLocationTypes;
using MediFix.Application.Locations.SetActiveStatus;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

[Authorize]
public class LocationsController(ISender sender) : ApiController
{
    [HttpGet("types")]
    public async Task<IActionResult> GetLocationTypes(CancellationToken cancellationToken)
    {
        var query = new GetLocationTypesRequest();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpGet("types/{locationType:int}")]
    public async Task<IActionResult> GetByLocationType(byte locationType, CancellationToken cancellationToken)
    {
        var query = new GetLocationsByTypeRequest((LocationType)locationType);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var locationId = LocationId.From(id);
        var query = new GetLocationRequest(locationId);

        var locationResult = await sender.Send(query, cancellationToken);

        return locationResult.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}/children")]
    public async Task<IActionResult> GetChildren(Guid id, CancellationToken cancellationToken)
    {
        var locationId = LocationId.From(id);
        var query = new GetLocationChildrenRequest(locationId);

        var locationsResult = await sender.Send(query, cancellationToken);

        return locationsResult.Match(Ok, Problem);
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var createResult = await sender.Send(request, cancellationToken);

        return createResult.Match(value =>
            CreatedAtAction(nameof(GetById), value),
            Problem);
    }

    [HttpPatch("{id:guid}/active/{isActive:bool}")]
    public async Task<IActionResult> Activate(Guid id, bool isActive, CancellationToken cancellationToken)
    {
        var locationId = LocationId.From(id);
        var command = new SetLocationActiveStatusCommand(locationId, isActive);

        var updateResult = await sender.Send(command, cancellationToken);

        return updateResult.Match(Ok, Problem);
    }

    [HttpPatch("{id:guid}/name/{newName}")]
    public async Task<IActionResult> ChangeName(Guid id, string newName, CancellationToken cancellationToken)
    {
        var locationId = LocationId.From(id);
        var query = new ChangeLocationNameCommand(locationId, newName);

        var locationsResult = await sender.Send(query, cancellationToken);

        return locationsResult.Match(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var locationId = LocationId.From(id);
        var request = new DeleteLocationCommand(locationId);

        var deleteResult = await sender.Send(request, cancellationToken);

        return deleteResult.Match(NoContent, Problem);
    }
}