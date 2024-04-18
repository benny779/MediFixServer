using MediFix.Application.Locations;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class LocationsRepository(ApplicationDbContext dbContext) : ILocationsRepository
{
    public async Task<Result<Location>> GetByIdAsync(LocationId locationId, CancellationToken cancellationToken = default)
    {
        var location = await dbContext.Locations.FindAsync([locationId], cancellationToken);

        if (location is null)
        {
            return Error.EntityNotFound<Location>(locationId.Value);
        }

        return location;
    }

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

    public async Task<Result> InsertAsync(Location location, CancellationToken cancellationToken = default)
    {
        dbContext.Locations.Add(location);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(Location location, CancellationToken cancellationToken = default)
    {
        dbContext.Locations.Update(location);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Location location, CancellationToken cancellationToken = default)
    {
        dbContext.Locations.Remove(location);

        var rowsDeleted = await dbContext.SaveChangesAsync(cancellationToken);

        return rowsDeleted > 0 ? Result.Success() : Error.EntityNotFound<Location>(location.Id);
    }

    public async Task<Result> DeleteAsync(LocationId locationId, CancellationToken cancellationToken = default)
    {
        var rowsDeleted = await dbContext.Locations
              .Where(l => l.Id == locationId)
              .ExecuteDeleteAsync(cancellationToken);

        return rowsDeleted > 0 ? Result.Success() : Error.EntityNotFound<Location>(locationId);
    }
}
