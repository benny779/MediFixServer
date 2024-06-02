using MediFix.Application.Users;
using MediFix.Application.Users.Entities;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class ApplicationUserRepository(ApplicationDbContext dbContext)
    : IApplicationUserRepository
{
    public void Update(ApplicationUser user)
    {
        dbContext.Users.Update(user);
    }
}
