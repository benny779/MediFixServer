namespace MediFix.Domain.Users;

public record ManagerId(Guid Value) : StronglyTypedId<Guid>(Value);