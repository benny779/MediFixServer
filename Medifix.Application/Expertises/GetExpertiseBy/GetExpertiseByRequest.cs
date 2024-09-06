using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Expertises.GetExpertiseBy;

public record GetExpertiseByRequest(
    Guid? CategoryId,
    Guid? PractitionerId) : IQuery<ExpertisesResponse>;