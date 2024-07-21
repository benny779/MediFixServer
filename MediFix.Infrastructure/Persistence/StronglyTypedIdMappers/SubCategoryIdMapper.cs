using Dapper;
using MediFix.Domain.Categories;
using System.Data;

namespace MediFix.Infrastructure.Persistence.StronglyTypedIdMappers;

internal sealed class SubCategoryIdMapper : SqlMapper.TypeHandler<SubCategoryId>
{
    public override void SetValue(IDbDataParameter parameter, SubCategoryId? value)
    {
        parameter.Value = value is not null ? value.Value : DBNull.Value;
    }

    public override SubCategoryId Parse(object value)
    {
        return SubCategoryId.From((Guid)value);
    }
}
