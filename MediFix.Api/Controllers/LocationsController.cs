﻿using MediatR;
using MediFix.Application.Locations.ChangeLocationName;
using MediFix.Application.Locations.CreateLocation;
using MediFix.Application.Locations.DeleteLocation;
using MediFix.Application.Locations.GetLocation;
using MediFix.Application.Locations.GetLocationChildren;
using MediFix.Application.Locations.GetLocationTypes;
using MediFix.Application.Locations.SetActiveStatus;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class LocationsController(ISender sender) : ApiController
{
    [HttpGet("types")]
    public async Task<IActionResult> GetLocationTypes(CancellationToken cancellationToken)
    {
        var query = new GetLocationTypesRequest();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] bool includeParents, CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
        var query = new GetLocationRequest(locationId, includeParents);

        var locationResult = await sender.Send(query, cancellationToken);

        return locationResult.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}/children")]
    public async Task<IActionResult> GetChildren(Guid id, CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
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

    [HttpPost("{id:guid}/active/{isActive:bool}")]
    public async Task<IActionResult> Activate(Guid id, bool isActive, CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
        var command = new SetLocationActiveStatusCommand(locationId, isActive);

        var updateResult = await sender.Send(command, cancellationToken);

        return updateResult.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}/name/{newName}")]
    public async Task<IActionResult> ChangeName(Guid id, string newName, CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
        var query = new ChangeLocationNameCommand(locationId, newName);

        var locationsResult = await sender.Send(query, cancellationToken);

        return locationsResult.Match(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
        var request = new DeleteLocationCommand(locationId);

        var deleteResult = await sender.Send(request, cancellationToken);

        return deleteResult.Match(NoContent, Problem);
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
}