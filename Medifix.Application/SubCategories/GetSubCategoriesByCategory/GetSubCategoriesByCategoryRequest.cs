using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Categories;
using MediFix.Application.Categories.GetCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediFix.Application.SubCategories.GetSubCategoriesByCategory;

public record GetSubCategoriesByCategoryRequest(Guid CategoryId)
    : IQuery<SubCategoriesResponse>;