using MediFix.Domain.Core.Primitives;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediFix.Infrastructure.Persistence.Converters;

internal class StronglyTypedIdValueConverter<TStronglyTypedId, TValue>
    : ValueConverter<TStronglyTypedId, TValue>
    where TStronglyTypedId : StronglyTypedId<TValue>
    where TValue : notnull
{
    public StronglyTypedIdValueConverter()
        : base(
            id => ConvertToValue(id),
            value => ConvertToStronglyTypedId(value))
    {
    }

    private static TValue ConvertToValue(TStronglyTypedId id)
    {
        return id.Value;
    }

    private static TStronglyTypedId ConvertToStronglyTypedId(TValue value)
    {
        var factory = StronglyTypedIdHelper.GetFactory<TValue>(typeof(TStronglyTypedId));
        return (TStronglyTypedId)factory(value);
    }
}