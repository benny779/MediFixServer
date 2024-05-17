using MediFix.Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence.Seed;

internal static class LocationsSeed
{
    public static ModelBuilder SeedLocations(this ModelBuilder modelBuilder)
    {
        var locations = GetLocations();

        modelBuilder.Entity<Location>()
            .HasData(locations);

        return modelBuilder;
    }

    private static IEnumerable<Location> GetLocations()
    {
        var buildingA = Location.Create(
            LocationId.Create(),
            LocationType.Building,
            "A")
            .Value!;

        var floor0 = Location.Create(
            LocationId.Create(),
            LocationType.Floor,
            "0",
            buildingA)
            .Value!;

        var depHr = Location.Create(
            LocationId.Create(),
            LocationType.Department,
            "HR",
            floor0)
            .Value!;

        var depIt = Location.Create(
            LocationId.Create(),
            LocationType.Department,
            "IT",
            floor0)
            .Value!;

        var room100 = Location.Create(
            LocationId.Create(),
            LocationType.Room,
            "100",
            depHr)
            .Value!;

        var room101 = Location.Create(
            LocationId.Create(),
            LocationType.Room,
            "101",
            depHr)
            .Value!;

        var room102 = Location.Create(
            LocationId.Create(),
            LocationType.Room,
            "102",
            depHr)
            .Value!;

        var room200 = Location.Create(
            LocationId.Create(),
            LocationType.Room,
            "200",
            depIt)
            .Value!;

        var room201 = Location.Create(
            LocationId.Create(),
            LocationType.Room,
            "201",
            depIt)
            .Value!;

        var room202 = Location.Create(
            LocationId.Create(),
            LocationType.Room,
            "202",
            depIt)
            .Value!;

        return [buildingA, floor0, depHr, depIt, room100, room101, room102, room200, room201, room202];
    }
}
