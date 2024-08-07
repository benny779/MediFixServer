using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Categories.AddCategoryExpertise;

public record AddCategoryExpertiseCommand(
    Guid CategoryId,
    Guid ExpertiseId) : ICommand;