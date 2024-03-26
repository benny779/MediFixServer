using MediFix.Application.Users;
using MediFix.Domain.Users;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUsersRepository
{
    public async Task<Result> Add(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(user, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}