using MediFix.Application.Locations;
using MediFix.Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class LocationsRepository(DbContext dbContext) : ILocationsRepository
{
    public async Task<Result<Location>> GetByIdAsync(LocationId locationId, CancellationToken cancellationToken = default)
    {
        var location = await dbContext
            .FindAsync<Location>(locationId);

        if (location is null)
        {
            return Error.EntityNotFound<Location>(locationId.Value);
        }

        return location;
    }

    public async Task<Result> AddAsync(Location location, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(location, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
