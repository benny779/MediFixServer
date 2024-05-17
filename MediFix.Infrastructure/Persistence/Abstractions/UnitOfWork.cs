using MediFix.Application.Abstractions.Data;

namespace MediFix.Infrastructure.Persistence.Abstractions;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
