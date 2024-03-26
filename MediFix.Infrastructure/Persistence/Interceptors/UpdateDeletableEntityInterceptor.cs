using MediFix.Domain.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MediFix.Infrastructure.Persistence.Interceptors;

internal sealed class UpdateDeletableEntityInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<IDeletableEntity>> deletableEntities =
            dbContext
                .ChangeTracker
                .Entries<IDeletableEntity>();

        var now = DateTime.Now;

        foreach (var deletableEntity in deletableEntities)
        {
            if (deletableEntity.State == EntityState.Deleted)
            {
                deletableEntity.State = EntityState.Modified;
                deletableEntity.Property(p => p.DeletedOn).CurrentValue = now;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}