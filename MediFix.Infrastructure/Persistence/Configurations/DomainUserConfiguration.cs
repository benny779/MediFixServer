//using MediFix.Application.Users.Entities;
//using MediFix.Domain.Users;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace MediFix.Infrastructure.Persistence.Configurations;

//internal class DomainUserConfiguration : IEntityTypeConfiguration<DomainUser>
//{
//    public void Configure(EntityTypeBuilder<DomainUser> builder)
//    {
//        builder.HasKey(u => u.Id);

//        builder.Property(u => u.Id)
//            .HasConversion(
//                id => id.Value,
//                value => UserId.From(value));

//        builder.Property(u => u.Type)
//            .HasConversion<byte>();

//        builder.HasOne<ApplicationUser>()
//            .WithOne()
//            .HasForeignKey<DomainUser>(u => u.Id);
//    }
//}