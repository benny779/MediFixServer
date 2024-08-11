using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Expertises;

public record ExpertisesResponse(IEnumerable<ExpertiseResponse> Items) : IListResponse<ExpertiseResponse>;