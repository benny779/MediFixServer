﻿using MediFix.Domain.Locations.Abstract;
using MediFix.Domain.ServiceCalls;

namespace MediFix.Domain.Locations;

public class Department(LocationId id, string name, LocationId parentId)
    : BaseSubLocation(id, name, parentId)
{
    public override string LocationType => nameof(Department);
}