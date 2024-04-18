using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations;

public interface ILocationsRepository : IRepository<Location, LocationId>
{
    Task<Result<List<Location>>> GetByIdWithParentsAsync(LocationId locationId, CancellationToken cancellationToken = default);
    Task<Result<List<Location>>> GetChildren(LocationId locationId, CancellationToken cancellationToken = default);
}