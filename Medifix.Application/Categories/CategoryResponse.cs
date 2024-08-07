using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Expertises;
using MediFix.Domain.Categories;

namespace MediFix.Application.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    bool IsActive,
    List<ExpertiseResponse> AllowedExpertises) : ICreatedResponse
{
    public static CategoryResponse FromDomainCategory(Category category)
    {
        return new CategoryResponse(
            category.Id,
            category.Name,
            category.IsActive,
            category
                .AllowedExpertises
                .Select(e => new ExpertiseResponse(e.Id, e.Name))
                .ToList());
    }
}