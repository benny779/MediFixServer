namespace MediFix.Domain.Practitioners;

public record ExpertiseId(Guid Value) : StronglyTypedId<Guid>(Value);