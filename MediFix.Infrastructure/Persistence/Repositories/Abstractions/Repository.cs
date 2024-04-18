using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories.Abstractions;

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
    public async Task<Result> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<TEntity>().Add(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<TEntity>().Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Set<TEntity>().Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var rowsDeleted = await dbContext.Set<TEntity>()
            .Where(l => l.Id.Equals(id))
            .ExecuteDeleteAsync(cancellationToken);

        return rowsDeleted > 0 ? Result.Success() : Error.EntityNotFound<TEntity>(id);
    }
}