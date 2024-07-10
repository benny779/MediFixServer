using MediFix.Application.Users;
using MediFix.Domain.Users;
using MediFix.Infrastructure.Persistence.Abstractions;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class ManagerRepository(ApplicationDbContext dbContext)
    : Repository<Manager, ManagerId>(dbContext)
        , IManagerRepository
{
    public override IQueryable<Manager> GetQueryableWithNavigation()
    {
        return GetQueryable();
    }
}