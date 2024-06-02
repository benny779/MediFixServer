using MediFix.Application.Users.Entities;
using MediFix.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFix.Infrastructure.Persistence.Configurations;

internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                managerId => managerId.Value,
                value => ClientId.From(value));
    }
}