using MediFix.Domain.Locations.Abstract;

namespace MediFix.Domain.Locations;

public class Room(LocationId id, string name, LocationId parentId)
    : BaseSubLocation(id, name, parentId)
{
    public override string LocationType => nameof(Room);
}