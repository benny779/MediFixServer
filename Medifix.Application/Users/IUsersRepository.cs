using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users;

public interface IUsersRepository
{
    Task<Result> Add(User user, CancellationToken cancellationToken = default);
}