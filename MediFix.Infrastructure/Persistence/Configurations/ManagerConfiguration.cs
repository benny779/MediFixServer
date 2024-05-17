using MediFix.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        ConfigurePractitioner(builder);
    }


    private static void ConfigurePractitioner(EntityTypeBuilder<Manager> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                practitionerId => practitionerId.Value,
                value => new ManagerId(value));

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Manager>(p => p.UserId);
    }
}