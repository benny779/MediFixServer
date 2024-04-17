using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations;

public interface ILocationsRepository
{
    Task<Result<Location>> GetByIdAsync(LocationId locationId, CancellationToken cancellationToken = default);
    Task<Result<List<Location>>> GetByIdWithParentsAsync(LocationId locationId, CancellationToken cancellationToken = default);
    Task<Result<List<Location>>> GetChildren(LocationId locationId, CancellationToken cancellationToken = default);
    Task<Result> InsertAsync(Location location, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Location location, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Location location, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(LocationId locationId, CancellationToken cancellationToken = default);
}