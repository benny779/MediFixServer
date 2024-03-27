using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations;

public interface ILocationsRepository
{
    Task<Result<Location>> GetByIdAsync(LocationId locationId, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(Location location, CancellationToken cancellationToken = default);
}