using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Expertises;

public record ExpertiseResponse(
    Guid Id,
    string Name) : ICreatedResponse;