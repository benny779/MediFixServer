using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Expertises.CreateExpertise;

public record CreateExpertiseCommand(string Name) : ICommand<ExpertiseResponse>;