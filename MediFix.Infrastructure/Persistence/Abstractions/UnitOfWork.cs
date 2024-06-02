using MediFix.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace MediFix.Infrastructure.Persistence.Abstractions;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbTransaction> BeginTransactionAsync()
    {
        var dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
        return dbContextTransaction.GetDbTransaction();
    }
}
