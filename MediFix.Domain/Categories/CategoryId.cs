namespace MediFix.Domain.Categories;

public record CategoryId(Guid Value) : StronglyTypedId<Guid>(Value);