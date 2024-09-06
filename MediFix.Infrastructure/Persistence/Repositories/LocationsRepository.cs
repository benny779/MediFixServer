using MediFix.Application.Locations;
using MediFix.Domain.Locations;
using MediFix.Infrastructure.Persistence.Abstractions;
using MediFix.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class LocationsRepository(ApplicationDbContext dbContext)
    : Repository<Location, LocationId>(dbContext), ILocationsRepository
{
    public async Task<Result<List<Location>>> GetByType(LocationType locationType, CancellationToken cancellationToken = default)
    {
        var locations = await dbContext
            .Locations
            .Where(loc => loc.LocationType == locationType)
            .ToListAsync(cancellationToken);

        if (locations.IsEmpty())
        {
            return Error.NotFound(
                "Locations.Type.NotFound",
                $"No location with found for the location type '{locationType}'");
        }

        return locations;
    }

    public async Task<Result<List<Location>>> GetChildren(LocationId locationId, CancellationToken cancellationToken = default)
    {
        var locations = await dbContext
            .Locations
            .Where(loc => loc.ParentId == locationId)
            .ToListAsync(cancellationToken);

        if (locations.IsEmpty())
        {
            return Error.NotFound(
                "Locations.Children.NotFound",
                $"No children found for the location with id '{locationId.Value}'");
        }

        return locations;
    }

    public Task<bool> SameTypeAndNameAlreadyExist(Location location, CancellationToken cancellationToken = default)
    {
        return ExistsAsync(loc =>
                loc.Id != location.Id &&
                loc.LocationType == location.LocationType &&
                loc.ParentId == location.ParentId &&
                loc.Name.Equals(location.Name),
            cancellationToken);
    }

    public override IQueryable<Location> GetQueryableWithNavigation()
    {
        return GetQueryable();
    }
}
