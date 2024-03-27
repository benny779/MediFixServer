using MediatR;
using MediFix.Application.Locations;
using MediFix.Application.Locations.CreateLocation;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        ILocationsRepository locationsRepository,
        CancellationToken cancellationToken)
    {
        var locationId = new LocationId(id);
        var locationResult = await locationsRepository.GetByIdAsync(locationId, cancellationToken);

        return locationResult.IsFailure
            ? NotFound(Error.EntityNotFound<Location>(id))
            : Ok(locationResult.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var createResult = await sender.Send(request, cancellationToken);

        return createResult.IsFailure
            ? BadRequest(createResult.Error)
            : CreatedAtAction(
                nameof(GetById),
                createResult.Value!);
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