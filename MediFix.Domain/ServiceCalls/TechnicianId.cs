using MediFix.Domain.Core.Primitives;

namespace MediFix.Domain.ServiceCalls;

public record TechnicianId(Guid Value) : StronglyTypedId<Guid>(Value);