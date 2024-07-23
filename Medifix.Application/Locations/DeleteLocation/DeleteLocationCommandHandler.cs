using Dapper;
using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.DeleteLocation;

internal sealed class DeleteLocationCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ILocationsRepository locationsRepository)
    : ICommandHandler<DeleteLocationCommand>
{
    public async Task<Result> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
    {
        if (!await LocationCanBeDeleted(request.LocationId))
        {
            return Error.Validation(
                "Location.DeleteNotAllowed",
                "The location cannot be deleted.");
        }

        return await locationsRepository.DeleteByIdAsync(request.LocationId, cancellationToken);
    }

    private async Task<bool> LocationCanBeDeleted(LocationId locationId)
    {
        using var dbConnection = dbConnectionFactory.CreateOpenConnection();

        const string sql = """
                           SELECT	Count( sc.Id ) AS c
                           FROM	dbo.ServiceCalls AS sc
                           INNER JOIN dbo.Locations AS l
                           ON sc.LocationId = l.Id
                           WHERE l.Id = @LocationId
                           """;

        var result = await dbConnection.ExecuteScalarAsync<int>(sql, new { LocationId = locationId });

        return result == 0;
    }
}