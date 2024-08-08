using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.Practitioners.GetPractitioners;

public record GetPractitionersRequest : IQuery<PractitionersResponse>;