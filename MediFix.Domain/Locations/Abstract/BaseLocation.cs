using MediFix.Domain.Core.Primitives;
using MediFix.Domain.ServiceCalls;

namespace MediFix.Domain.Locations.Abstract;

public abstract class BaseLocation : Entity<LocationId>
{
    public abstract string LocationType { get; }

    public string Name { get; private set; }

    protected BaseLocation(LocationId id, string name) : base(id)
    {
        Name = name;
    }
}