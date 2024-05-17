namespace MediFix.Domain.Categories;

public record SubCategoryId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static SubCategoryId Create() => new(Guid.NewGuid());
}