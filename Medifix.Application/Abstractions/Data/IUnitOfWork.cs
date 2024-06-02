using System.Data;

namespace MediFix.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbTransaction> BeginTransactionAsync();
}
