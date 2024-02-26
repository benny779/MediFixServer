using MediFix.Domain.Core.Primitives;

namespace MediFix.Domain.Users;

public record UserId(Guid Value) : StronglyTypedId<Guid>(Value);