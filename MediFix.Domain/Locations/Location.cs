namespace MediFix.Domain.Locations;

public class Location : Entity<LocationId>
{
    public const int NameMaxLength = 30;


    public LocationType LocationType { get; init; }

    public string Name { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public LocationId? ParentId { get; set; }

    public Location? Parent { get; private set; }


    private Location()
    {
    }

    private Location(LocationId locationId) : base(locationId)
    {
    }

    public static Result<Location> Create(
        LocationId locationId,
        LocationType locationType,
        string name,
        Location? parent = null)
    {
        if (!IsValidName(name))
        {
            return ErrorInvalidName;
        }

        if (locationType == LocationType.Building)
        {
            if (parent is not null)
            {
                return Error.Validation("building with parent");
            }

            return new Location(locationId)
            {
                LocationType = locationType,
                IsActive = true,
                Name = name,
            };
        }

        if (parent is null)
        {
            return Error.Validation("location without parent");
        }

        var currentTypeValue = (byte)locationType;
        var parentTypeValue = (byte)parent.LocationType;

        if (currentTypeValue != parentTypeValue + 1)
        {
            return Error.Validation("location with inallowed parent");
        }

        return new Location(locationId)
        {
            LocationType = locationType,
            Name = name,
            IsActive = true,
            Parent = parent,
            ParentId = parent.Id
        };
    }
    public void SetActiveStatus(bool isActive) => IsActive = isActive;

    private static bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    private static readonly Error ErrorInvalidName =
        Error.Validation(
            "Location.InvalidName",
            "A location name cannot be null or empty.");

    public Result ChangeName(string name)
    {
        if (!IsValidName(name))
        {
            return ErrorInvalidName;
        }
        
        Name = name;
        return Result.Success();
    }
}

public enum LocationType : byte
{
    Building = 1,
    Floor,
    Department,
    Room
}