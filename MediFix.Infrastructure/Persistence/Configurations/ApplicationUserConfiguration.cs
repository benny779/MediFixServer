using MediFix.Application.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(ApplicationUser.FirstNameMaxLength);

        builder.Property(u => u.LastName)
            .HasMaxLength(ApplicationUser.LastNameMaxLength);

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(450);

        builder.Property(u => u.Type)
            .HasConversion<byte>();
    }
}