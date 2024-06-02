using MediFix.Application.Users.Entities;

namespace MediFix.Application.Users;

public interface IApplicationUserRepository
{
    void Update(ApplicationUser user);
}