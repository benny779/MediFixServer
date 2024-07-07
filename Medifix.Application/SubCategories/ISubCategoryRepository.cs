using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Categories;

namespace MediFix.Application.SubCategories;

public interface ISubCategoryRepository : IRepository<SubCategory, SubCategoryId>;