using MediFix.Application.Users;
using MediFix.Domain.Users;
using MediFix.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class PractitionerRepository(ApplicationDbContext dbContext)
    : Repository<Practitioner, PractitionerId>(dbContext)
        , IPractitionerRepository
{
    public override IQueryable<Practitioner> GetQueryableWithNavigation()
    {
        return dbContext
            .Practitioners
            .Include(p => p.Expertises);
    }
}