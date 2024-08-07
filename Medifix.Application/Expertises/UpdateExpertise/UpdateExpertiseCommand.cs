using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Expertises.UpdateExpertise;

public record UpdateExpertiseCommand(
    Guid ExpertiseId,
    string Name) : ICommand;