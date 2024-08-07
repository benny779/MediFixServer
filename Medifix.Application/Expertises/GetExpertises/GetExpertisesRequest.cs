using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Expertises.GetExpertises;

public record GetExpertisesRequest : IQuery<ExpertisesResponse>;