using MediFix.Application.Expertises;
using MediFix.Application.Users;
using MediFix.Application.Users.Practitioners;
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

    public IQueryable<PractitionerResponse> GetResponseQueryable()
    {
        return GetResponseQueryable(GetQueryableWithNavigation());
    }

    public IQueryable<PractitionerResponse> GetResponseQueryable(IQueryable<Practitioner> queryable)
    {
        return queryable
            .AsNoTracking()
            .Join(dbContext.Users,
                practitioner => practitioner.Id,
                appUser => appUser.Id,
                (practitioner, appUser) => new PractitionerResponse(
                    practitioner.Id,
                    appUser.FirstName,
                    appUser.LastName,
                    appUser.FullName,
                    appUser.Email!,
                    appUser.PhoneNumber,
                    practitioner.Expertises.Select(exp => new ExpertiseResponse(exp.Id, exp.Name)))
            );
    }
}