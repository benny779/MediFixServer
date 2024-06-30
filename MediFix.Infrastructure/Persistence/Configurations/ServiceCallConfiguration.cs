using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using MediFix.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class ServiceCallConfiguration : IEntityTypeConfiguration<ServiceCall>
{
    public void Configure(EntityTypeBuilder<ServiceCall> builder)
    {
        ConfigureServiceCall(builder);

        ConfigureStatusHistory(builder);
    }

    private static void ConfigureServiceCall(EntityTypeBuilder<ServiceCall> builder)
    {
        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.Id)
            .HasConversion<StronglyTypedIdValueConverter<ServiceCallId, Guid>>();

        builder.Property(sc => sc.Status)
            .HasConversion<byte>();

        builder.Property(sc=> sc.PractitionerId)
            .HasConversion(
                practitionerId => practitionerId.Value,
                value => PractitionerId.From(value));

        builder.Property(sc => sc.Priority)
            .HasConversion<byte>();

        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(sc => sc.ClientId);

        builder.HasOne<Location>()
            .WithMany()
            .HasForeignKey(sc => sc.LocationId);

        builder.HasOne<SubCategory>()
            .WithMany()
            .HasForeignKey(sc => sc.SubCategoryId);

        builder.HasOne<Practitioner>()
            .WithMany()
            .HasForeignKey(sc => sc.PractitionerId);
    }

    private static void ConfigureStatusHistory(EntityTypeBuilder<ServiceCall> builder)
    {
        builder.OwnsMany(sc => sc.StatusHistory, sb =>
        {
            sb.HasKey(s => new { s.ServiceCallId, s.DateTime });

            sb.WithOwner().HasForeignKey(s => s.ServiceCallId);

            sb.Property(s => s.ServiceCallId)
                .HasConversion(
                    serviceCallId => serviceCallId.Value,
                    value => ServiceCallId.From(value));

            sb.HasOne<Practitioner>()
                .WithMany()
                .HasForeignKey(s => s.PractitionerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            sb.Property(s => s.PractitionerId)
                .HasConversion(
                    practitionerId => practitionerId.Value,
                    value => PractitionerId.From(value));

            sb.Property(s => s.Status)
                .HasConversion<byte>();
        });
    }
}