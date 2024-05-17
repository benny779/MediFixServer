namespace MediFix.Domain.Users;

public record ExpertiseId(Guid Value) : StronglyTypedId<Guid>(Value);