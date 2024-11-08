﻿using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations;

public interface ILocationsRepository : IRepository<Location, LocationId>
{
    Task<Result<List<Location>>> GetByType(LocationType locationType, bool withInactive, CancellationToken cancellationToken = default);
    Task<Result<List<Location>>> GetChildren(LocationId locationId, bool withInactive, CancellationToken cancellationToken = default);
    Task<bool> SameTypeAndNameAlreadyExist(Location location, CancellationToken cancellationToken = default);
}