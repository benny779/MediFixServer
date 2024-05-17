using MediFix.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Seed;

internal static class UsersSeed
{
    public static ModelBuilder SeedUsers(this ModelBuilder modelBuilder)
    {
        var users = GetUsers();

        modelBuilder.Entity<User>()
            .HasData(users);

        return modelBuilder;
    }

    private static IEnumerable<User> GetUsers()
    {
        return [new User(UserId.Create())];
    }
}
