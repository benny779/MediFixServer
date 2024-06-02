using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Categories;

namespace MediFix.Application.Categories;

public interface ICategoryRepository : IRepository<Category, CategoryId>;