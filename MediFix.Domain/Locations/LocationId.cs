using MediFix.Domain.Core.Primitives;

namespace MediFix.Domain.Locations;

public record LocationId(Guid Value) : StronglyTypedId<Guid>(Value);