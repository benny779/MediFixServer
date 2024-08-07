using MediFix.Domain.Categories;
using MediFix.Domain.Expertises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                categoryId => categoryId.Value,
                value => CategoryId.From(value));

        builder.Property(c => c.Name)
            .HasMaxLength(Category.NameMaxLength);

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasMany(c => c.AllowedExpertises)
            .WithMany(e => e.Categories)
            .UsingEntity(join =>
                join.ToTable($"{nameof(Category)}{nameof(Expertise)}"));

        builder.Navigation(c => c.AllowedExpertises)
            .AutoInclude();
    }
}