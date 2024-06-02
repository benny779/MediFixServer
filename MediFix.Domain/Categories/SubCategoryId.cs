namespace MediFix.Domain.Categories;

public record SubCategoryId : StronglyTypedId<Guid>
{
    private SubCategoryId(Guid value) : base(value)
    {
    }

    public static SubCategoryId Create() => new(Guid.NewGuid());
    public static SubCategoryId From(Guid value) => new(value);
}