using MediFix.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                subCategoryId => subCategoryId.Value,
                value => new SubCategoryId(value));

        builder.Property(s => s.Name)
            .HasMaxLength(SubCategory.NameMaxLength);

        builder.Property(s => s.CategoryId)
            .HasConversion(
                parentId => parentId.Value,
                value => new CategoryId(value));

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(s => s.CategoryId);

        builder
            .HasIndex(s => new { s.Name, s.CategoryId })
            .IsUnique();
    }
}