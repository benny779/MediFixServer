using MediFix.Domain.Core.Primitives;

namespace MediFix.Domain.ServiceCalls;

public record LocationId(Guid Value) : StronglyTypedId<Guid>(Value);