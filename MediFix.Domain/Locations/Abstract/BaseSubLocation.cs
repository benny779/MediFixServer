using MediFix.Domain.ServiceCalls;

namespace MediFix.Domain.Locations.Abstract;

public abstract class BaseSubLocation : BaseLocation
{
    public LocationId ParentId { get; set; }

    protected BaseSubLocation(LocationId id, string name, LocationId parentId) : base(id, name)
    {
        ParentId = parentId;
    }

}