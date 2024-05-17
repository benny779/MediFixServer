namespace MediFix.Domain.Users;

public record PractitionerId(Guid Value) : StronglyTypedId<Guid>(Value);