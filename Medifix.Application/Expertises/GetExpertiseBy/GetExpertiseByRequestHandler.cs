using Dapper;
using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Expertises.GetExpertiseBy;

internal sealed class GetExpertiseByRequestHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetExpertiseByRequest, ExpertisesResponse>
{
    public async Task<Result<ExpertisesResponse>> Handle(GetExpertiseByRequest request, CancellationToken cancellationToken)
    {
        using var dbConnection = dbConnectionFactory.CreateOpenConnection();

        const string sql = """
                            SELECT DISTINCT	e.Id, e.Name
                            FROM	dbo.Expertises AS e
                            LEFT OUTER JOIN dbo.CategoryExpertise AS ce
                            ON e.Id = ce.AllowedExpertisesId
                            LEFT OUTER JOIN dbo.PractitionerExpertise AS pe
                            ON e.Id = pe.ExpertisesId
                            WHERE (@CategoryId IS NULL OR ce.CategoriesId = @CategoryId)
                            AND   (@PractitionerId IS NULL OR pe.PractitionersId = @PractitionerId)
                            """;

        var result = await dbConnection.QueryAsync<ExpertiseResponse>(
            sql,
            new { request.CategoryId, request.PractitionerId });

        return new ExpertisesResponse(result);
    }
}