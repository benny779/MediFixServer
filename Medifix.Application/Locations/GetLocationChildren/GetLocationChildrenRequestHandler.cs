﻿using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.GetLocationChildren;

internal sealed class GetLocationChildrenRequestHandler(
    ILocationsRepository locationsRepository)
    : IQueryHandler<GetLocationChildrenRequest, GetLocationChildrenResponse>
{
    public async Task<Result<GetLocationChildrenResponse>> Handle(
        GetLocationChildrenRequest request,
        CancellationToken cancellationToken)
    {
        var locationsResult = await locationsRepository
            .GetChildren(request.LocationId, request.WithInactive, cancellationToken);

        if (locationsResult.IsFailure)
        {
            return locationsResult.Error;
        }

        var locations = locationsResult.Value;

        var locationType = locations
            .Select(loc => loc.LocationType)
            .FirstOrDefault();

        var list = locations
            .Select(LocationResponse.FromDomainLocation)
            .OrderBy(loc => loc.Name)
            .ToList();

        return new GetLocationChildrenResponse(locationType, list);
    }
}