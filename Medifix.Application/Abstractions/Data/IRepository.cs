using MediFix.Domain.Core.Primitives;
using MediFix.SharedKernel.Results;
using System.Linq.Expressions;

namespace MediFix.Application.Abstractions.Data;

public interface IRepository<TEntity, in TId>
    where TEntity : Entity<TId>
    where TId : class
{
    Task<Result<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<TEntity>> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<Result> DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);

    void Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);

    IQueryable<TEntity> GetQueryable();
    IQueryable<TEntity> GetQueryableWithNavigation();
    Task<Result<TEntity>> GetByIdWithNavigationAsync(TId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}