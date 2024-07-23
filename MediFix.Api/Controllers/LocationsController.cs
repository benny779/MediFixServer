using MediatR;
using MediFix.Application.Locations.CreateLocation;
using MediFix.Application.Locations.GetLocation;
using MediFix.Application.Locations.GetLocationChildren;
using MediFix.Application.Locations.GetLocationsByType;
using MediFix.Application.Locations.UpdateLocation;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

[Authorize]
public class LocationsController(ISender sender) : ApiController
{
    //[HttpGet("types")]
    //public async Task<IActionResult> GetLocationTypes(CancellationToken cancellationToken)
    //{
    //    var query = new GetLocationTypesRequest();

    //    var result = await sender.Send(query, cancellationToken);

    //    return result.Match(Ok, Problem);
    //}

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

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var updateResult = await sender.Send(request, cancellationToken);

        return updateResult.Match(Ok, Problem);
    }
}