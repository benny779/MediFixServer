using MediFix.Application.SubCategories;
using MediFix.Domain.Categories;
using MediFix.Infrastructure.Persistence.Abstractions;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class SubCategoryRepository(ApplicationDbContext dbContext)
    : Repository<SubCategory, SubCategoryId>(dbContext)
        , ISubCategoryRepository;