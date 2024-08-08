using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.Practitioners.DeletePractitionerExpertise;

public record DeletePractitionerExpertiseCommand(
    Guid PractitionerId,
    Guid ExpertiseId) : ICommand;
