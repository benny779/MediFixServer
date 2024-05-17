﻿using MediFix.Domain.Users;
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

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Practitioner>(p => p.UserId);

        builder.HasMany(p => p.Expertises)
            .WithMany(e => e.Practitioners)
            .UsingEntity(join =>
                join.ToTable($"{nameof(Practitioner)}{nameof(Expertise)}"));
    }
}