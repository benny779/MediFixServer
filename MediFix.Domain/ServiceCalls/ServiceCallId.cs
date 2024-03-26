namespace MediFix.Domain.ServiceCalls;

public record ServiceCallId(Guid Value) : StronglyTypedId<Guid>(Value);