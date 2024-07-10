using MediFix.Application.Categories;
using MediFix.Domain.Categories;
using MediFix.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext dbContext)
    : Repository<Category, CategoryId>(dbContext)
        , ICategoryRepository
{
    public override IQueryable<Category> GetQueryableWithNavigation()
    {
        return dbContext
            .Categories
            .Include(c => c.AllowedExpertises);
    }
}