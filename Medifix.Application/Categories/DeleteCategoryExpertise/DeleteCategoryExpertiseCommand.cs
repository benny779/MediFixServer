using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Categories.DeleteCategoryExpertise;

public record DeleteCategoryExpertiseCommand(
    Guid CategoryId,
    Guid ExpertiseId) : ICommand;
