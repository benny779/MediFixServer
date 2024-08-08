using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Users.Practitioners.GetPractitionerById;

public record GetPractitionerByIdRequest(Guid Id) : IQuery<PractitionerResponse>;