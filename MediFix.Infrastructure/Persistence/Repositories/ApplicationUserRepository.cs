using MediFix.Application.Users;
using MediFix.Application.Users.Entities;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class ApplicationUserRepository(ApplicationDbContext dbContext)
    : IApplicationUserRepository
{
    public void Insert(ApplicationUser entity)
    {
        dbContext.Users.Add(entity);
    }

    public void Update(ApplicationUser entity)
    {
        dbContext.Users.Update(entity);
    }

    public void Delete(ApplicationUser entity)
    {
        dbContext.Users.Remove(entity);
    }

    public IQueryable<ApplicationUser> GetQueryable()
    {
        return dbContext.Users;
    }

    public IQueryable<ApplicationUser> GetQueryableWithNavigation()
    {
        return dbContext.Users; 
    }
}