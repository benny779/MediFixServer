using MediFix.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Abstractions;

internal sealed class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
