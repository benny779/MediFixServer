using MediFix.Domain.Locations.Abstract;
using MediFix.Domain.ServiceCalls;

namespace MediFix.Domain.Locations;

public class Building(LocationId id, string name) 
    : BaseLocation(id, name)
{
    public override string LocationType => nameof(Building);
}