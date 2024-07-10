using MediFix.Application.SubCategories;
using MediFix.Domain.Categories;
using MediFix.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class SubCategoryRepository(ApplicationDbContext dbContext)
    : Repository<SubCategory, SubCategoryId>(dbContext)
        , ISubCategoryRepository
{
    public override IQueryable<SubCategory> GetQueryableWithNavigation()
    {
        return dbContext
            .SubCategories
            .Include(sc => sc.Category)
            .ThenInclude(c => c.AllowedExpertises);
    }
}