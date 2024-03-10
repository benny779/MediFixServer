using MediFix.Domain.Core.Primitives;

namespace MediFix.Domain.Practitioners;

public record ExpertiseId(Guid Value) : StronglyTypedId<Guid>(Value);