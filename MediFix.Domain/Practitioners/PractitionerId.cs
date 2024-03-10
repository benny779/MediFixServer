using MediFix.Domain.Core.Primitives;

namespace MediFix.Domain.Practitioners;

public record PractitionerId(Guid Value) : StronglyTypedId<Guid>(Value);