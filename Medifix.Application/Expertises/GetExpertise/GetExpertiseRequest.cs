using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Expertises.GetExpertise;

public record GetExpertiseRequest(Guid Id) : IQuery<ExpertiseResponse>;