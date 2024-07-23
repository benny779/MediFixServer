using Dapper;
using MediFix.Domain.Categories;
using System.Data;

namespace MediFix.Infrastructure.Persistence.StronglyTypedIdMappers;

internal sealed class CategoryIdMapper : SqlMapper.TypeHandler<CategoryId>
{
    public override void SetValue(IDbDataParameter parameter, CategoryId? value)
    {
        parameter.Value = value is not null ? value.Value : DBNull.Value;
    }

    public override CategoryId Parse(object value)
    {
        return CategoryId.From((Guid)value);
    }
}