using MediFix.Application.Locations;
using MediFix.Domain.Locations;
using MediFix.Infrastructure.Persistence.Abstractions;
using MediFix.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class LocationsRepository(ApplicationDbContext dbContext)
    : Repository<Location, LocationId>(dbContext), ILocationsRepository
{
    public async Task<Result<List<Location>>> GetByIdWithParentsAsync(LocationId locationId, CancellationToken cancellationToken = default)
    {
        string query = $"""
                         WITH cte
                         AS
                         (
                            SELECT	l.Id, l.LocationType, l.Name, l.ParentId, l.IsActive
                            FROM	dbo.Locations AS l
                            WHERE l.Id = '{locationId.Value.ToString()}'
                            UNION ALL
                            SELECT	l.Id, l.LocationType, l.Name, l.ParentId, l.IsActive
                            FROM	dbo.Locations AS l
                            INNER JOIN cte AS c
                            ON l.Id = c.ParentId
                         )
                         SELECT	cte.Id, cte.LocationType, cte.Name, cte.ParentId, cte.IsActive
                         FROM	cte
                         """;

        var locations = await dbContext.Locations
            .FromSqlRaw(query)
            .ToListAsync(cancellationToken);

        if (!locations.Any())
        {
            return Error.EntityNotFound<Location>(locationId.Value);
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

    public async Task<Result<bool>> SameTypeAndNameAlreadyExist(Location location, CancellationToken cancellationToken = default)
    {
        var locations = await dbContext
            .Locations
            .Where(loc => loc.Id != location.Id &&
                          loc.LocationType == location.LocationType &&
                          loc.Name.Equals(location.Name))
            .ToListAsync(cancellationToken);

        return locations.Count > 0;
    }
}
