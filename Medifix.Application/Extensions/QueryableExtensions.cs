using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.Extensions;

public static class QueryableExtensions
{
    public static async Task<Result<List<TSource>>> ResultToListAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        var result = await source.ToListAsync(cancellationToken);

        if (!result.Any())
        {
            return Error.NotFound($"{nameof(TSource)}.NotFound", $"No ${nameof(TSource)} found.");
        }

        return result;
    }
}