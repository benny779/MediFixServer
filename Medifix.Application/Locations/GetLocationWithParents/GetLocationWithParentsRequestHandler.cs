using Dapper;
using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Extensions;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.GetLocationWithParents;

internal sealed class GetLocationWithParentsRequestHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetLocationWithParentsRequest, LocationsWithTypeResponse>
{
    public async Task<Result<LocationsWithTypeResponse>> Handle(GetLocationWithParentsRequest request, CancellationToken cancellationToken)
    {
        const string query = $"""
                              WITH cte
                              AS
                              (
                                 SELECT	l.Id, l.LocationType, l.Name, l.ParentId, l.IsActive
                                 FROM	dbo.Locations AS l
                                 WHERE l.Id = @Id
                                 UNION ALL
                                 SELECT	l.Id, l.LocationType, l.Name, l.ParentId, l.IsActive
                                 FROM	dbo.Locations AS l
                                 INNER JOIN cte AS c
                                 ON l.Id = c.ParentId
                              )
                              SELECT	cte.Id, cte.LocationType, cte.Name, cte.ParentId, cte.IsActive
                              FROM	cte
                              """;

        using var connection = dbConnectionFactory.CreateOpenConnection();

        var locations = (await connection.QueryAsync<Location>(query, new { request.Id }))
            .ToList();

        if (locations.IsEmpty())
        {
            return Error.EntityNotFound<Location>(request.Id);
        }

        return new LocationsWithTypeResponse(
            locations
                .Select(l => new LocationWithTypeResponse(
                    l.Id,
                    l.LocationType,
                    l.Name,
                    l.IsActive))
                .OrderBy(l => l.LocationType));
    }
}