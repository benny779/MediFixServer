using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.Practitioners.AddPractitionerExpertise;

public record AddPractitionerExpertiseCommand(
    Guid PractitionerId,
    Guid ExpertiseId) : ICommand;