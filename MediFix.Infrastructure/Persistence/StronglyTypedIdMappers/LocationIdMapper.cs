using Dapper;
using MediFix.Domain.Locations;
using System.Data;

namespace MediFix.Infrastructure.Persistence.StronglyTypedIdMappers;

internal sealed class LocationIdMapper : SqlMapper.TypeHandler<LocationId>
{
    public override void SetValue(IDbDataParameter parameter, LocationId? value)
    {
        parameter.Value = value is not null ? value.Value : DBNull.Value;
    }

    public override LocationId Parse(object value)
    {
        return LocationId.From((Guid)value);
    }
}