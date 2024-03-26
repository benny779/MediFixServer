using MediFix.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value));

        builder.Property(u => u.FirstName)
            .HasMaxLength(User.FirstNameMaxLength);

        builder.Property(u => u.LastName)
            .HasMaxLength(User.LastNameMaxLength);

        builder.Property(u => u.Email)
            .HasMaxLength(255);

        builder.Property(u => u.Phone)
            .HasMaxLength(10);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Phone).IsUnique();
    }
}