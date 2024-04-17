using MediFix.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion(
                locationId => locationId.Value,
                value => new LocationId(value));

        builder.Property(l => l.ParentId)
            .HasConversion(
                locationId => locationId.Value,
                value => new LocationId(value));

        builder.Ignore(l => l.Parent);

        builder.Property(l => l.Name)
            .HasMaxLength(Location.NameMaxLength);

        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(l => l.ParentId);

        builder.HasIndex(b => new { b.Name, b.LocationType, b.ParentId })
            .IsUnique()
            .HasFilter(null);
    }
}