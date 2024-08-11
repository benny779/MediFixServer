using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.Practitioners;

public record PractitionersResponse(IEnumerable<PractitionerResponse> Items) : IListResponse<PractitionerResponse>;