using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Abstractions;

public abstract class Repository<TEntity, TId>(DbContext dbContext)
    : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : class
{
    public async Task<Result<TEntity>> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext
            .Set<TEntity>()
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);

        if (entity is null)
        {
            return Error.EntityNotFound<TEntity>(id);
        }

        return entity;
    }
    public void Insert(TEntity entity)
    {
        dbContext.Set<TEntity>().Add(entity);
    }
    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
    }
    public void Delete(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }
    public async Task<Result> DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var rowsDeleted = await dbContext.Set<TEntity>()
            .Where(l => l.Id.Equals(id))
            .ExecuteDeleteAsync(cancellationToken);

        return rowsDeleted > 0 ? Result.Success() : Error.EntityNotFound<TEntity>(id);
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return dbContext.Set<TEntity>();
    }
}