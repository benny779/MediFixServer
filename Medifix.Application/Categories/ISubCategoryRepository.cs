using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Categories;

namespace MediFix.Application.Categories;

public interface ISubCategoryRepository : IRepository<SubCategory, SubCategoryId>;