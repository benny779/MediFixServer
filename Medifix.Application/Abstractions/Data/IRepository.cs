using MediFix.Domain.Core.Primitives;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Abstractions.Data;

public interface IRepository<TEntity, in TId>
    where TEntity : Entity<TId>
{
    Task<Result<TEntity>> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<Result> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);
}