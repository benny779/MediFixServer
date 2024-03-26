using MediFix.Domain.Practitioners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class PractitionerConfiguration : IEntityTypeConfiguration<Practitioner>
{
    public void Configure(EntityTypeBuilder<Practitioner> builder)
    {
        ConfigurePractitioner(builder);
    }


    private static void ConfigurePractitioner(EntityTypeBuilder<Practitioner> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                practitionerId => practitionerId.Value,
                value => new PractitionerId(value));

        builder.Property(p => p.FirstName)
            .HasMaxLength(Practitioner.FirstNameMaxLength);

        builder.Property(p => p.LastName)
            .HasMaxLength(Practitioner.LastNameMaxLength);

        builder.Property(p => p.Email)
            .HasMaxLength(255);

        builder.Property(p => p.Phone)
            .HasMaxLength(10);

        builder.HasIndex(p => p.Email).IsUnique();
        builder.HasIndex(p => p.Phone).IsUnique();

        builder.Property(p => p.ExpertiseId)
            .HasConversion(
                expertiseId => expertiseId.Value,
                value => new ExpertiseId(value));

        builder.HasOne<Expertise>()
            .WithMany()
            .HasForeignKey(p => p.ExpertiseId);
    }
}