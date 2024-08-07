using MediFix.Application.Expertises;
using MediFix.Domain.Expertises;
using MediFix.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class ExpertiseRepository(ApplicationDbContext dbContext)
    : Repository<Expertise, ExpertiseId>(dbContext), IExpertiseRepository
{
    public override IQueryable<Expertise> GetQueryableWithNavigation()
    {
        return dbContext
            .Expertises
            .Include(e => e.Categories)
            .Include(e => e.Practitioners);
    }
}
