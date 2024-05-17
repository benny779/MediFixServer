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
            .ValueGeneratedNever()
            .HasConversion<StronglyTypedIdValueConverter<ServiceCallId, Guid>>();

        builder.Property(sc => sc.Priority)
            .HasConversion<byte>();

        builder.Property(sc => sc.Status)
            .HasConversion<byte>();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(sc => sc.UserId);

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
            sb.HasKey(s => s.Id);

            sb.Property(s => s.Id).UseIdentityColumn();

            sb.WithOwner().HasForeignKey(u => u.ServiceCallId);

            sb.Property(s => s.ServiceCallId)
                .HasConversion(
                    serviceCallId => serviceCallId.Value,
                    value => new ServiceCallId(value));

            sb.HasOne<User>()
                .WithMany()
                .HasForeignKey(s => s.UpdatedBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            sb.Property(s => s.UpdatedBy)
                .HasConversion(
                    userId => userId.Value,
                    value => new UserId(value));

            sb.HasOne<Practitioner>()
                .WithMany()
                .HasForeignKey(s => s.PractitionerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            sb.Property(s => s.PractitionerId)
                .HasConversion(
                    practitionerId => practitionerId.Value,
                    value => new PractitionerId(value));

            sb.Property(s => s.Status)
                .HasConversion<byte>();
        });
    }
}