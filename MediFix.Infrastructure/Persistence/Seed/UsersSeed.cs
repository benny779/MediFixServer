using MediFix.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Seed;

internal static class UsersSeed
{
    public static ModelBuilder SeedUsers(this ModelBuilder modelBuilder)
    {
        //var users = GetUsers();

        //modelBuilder.Entity<DomainUser>()
        //    .HasData(users);

        return modelBuilder;
    }

    //private static IEnumerable<DomainUser> GetUsers()
    //{
    //    return [new DomainUser(UserId.Create())];
    //}
}
