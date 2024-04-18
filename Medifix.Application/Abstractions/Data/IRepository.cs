using MediFix.Domain.Core.Primitives;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Abstractions.Data;

public interface IRepository<TEntity, in TId>
    where TEntity : Entity<TId>
{
    Task<Result<TEntity>> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    void Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<Result> DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);
}