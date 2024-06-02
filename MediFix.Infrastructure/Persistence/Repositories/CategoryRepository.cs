using MediFix.Application.Categories;
using MediFix.Domain.Categories;
using MediFix.Infrastructure.Persistence.Abstractions;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class CategoryRepository(ApplicationDbContext dbContext)
    : Repository<Category, CategoryId>(dbContext)
        , ICategoryRepository;