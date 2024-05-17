using MediFix.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class ExpertiseConfiguration : IEntityTypeConfiguration<Expertise>
{
    public void Configure(EntityTypeBuilder<Expertise> builder)
    {
        builder.HasKey(exp => exp.Id);

        builder.Property(exp => exp.Id)
            .HasConversion(
                expertiseId => expertiseId.Value,
                value => new ExpertiseId(value));

        builder.Property(exp => exp.Name)
            .HasMaxLength(Expertise.NameMaxLength);

        builder.HasIndex(exp => exp.Name).IsUnique();
    }
}