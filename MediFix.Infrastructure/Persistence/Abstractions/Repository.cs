using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MediFix.Infrastructure.Persistence.Abstractions;

public abstract class Repository<TEntity, TId>(DbContext dbContext)
    : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : class
{
    public async Task<Result<List<TEntity>>> GetAllAsync(CancellationToken cancellationToken)
    {
        List<TEntity> entities = await dbContext
            .Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (!entities.Any())
        {
            return Error.NotFound($"{nameof(TEntity)}.NotFound", $"No ${nameof(TEntity)} found.");
        }

        return entities;
    }

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

    public async Task<Result> DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var rowsDeleted = await dbContext.Set<TEntity>()
            .Where(l => l.Id.Equals(id))
            .ExecuteDeleteAsync(cancellationToken);

        return rowsDeleted > 0 ? Result.Success() : Error.EntityNotFound<TEntity>(id);
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
    public IQueryable<TEntity> GetQueryable()
    {
        return dbContext.Set<TEntity>();
    }

    public abstract IQueryable<TEntity> GetQueryableWithNavigation();
    
    public async Task<Result<TEntity>> GetByIdWithNavigationAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await GetQueryableWithNavigation()
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);

        if (entity is null)
        {
            return Error.EntityNotFound<TEntity>(id);
        }

        return entity;
    }

    public  Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<TEntity>()
            .AnyAsync(predicate, cancellationToken);
    }
}