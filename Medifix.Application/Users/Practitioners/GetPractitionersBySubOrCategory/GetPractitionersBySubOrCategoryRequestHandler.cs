using Dapper;
using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;
using MediFix.SharedKernel.Extensions;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users.Practitioners.GetPractitionersBySubOrCategory;

internal sealed class GetPractitionersBySubOrCategoryRequestHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetPractitionersBySubOrCategoryRequest, GetPractitionersBySubOrCategoryResponse>
{
    public async Task<Result<GetPractitionersBySubOrCategoryResponse>> Handle(
        GetPractitionersBySubOrCategoryRequest request,
        CancellationToken cancellationToken)
    {
        using var dbConnection = dbConnectionFactory.CreateOpenConnection();

        (string sql, object parameters) = GetQueryAndParameters(request);

        var practitioners = (await dbConnection.QueryAsync<PractitionerWithServiceCallCount>(
                sql,
                parameters))
            .ToList();

        if (practitioners.IsEmpty())
        {
            return Error.NotFound(
                "Practitioner.BySubOrCategory.NotFound",
                "No practitioners found.");
        }

        return new GetPractitionersBySubOrCategoryResponse(practitioners);
    }

    private static (string sql, object parameters) GetQueryAndParameters(GetPractitionersBySubOrCategoryRequest request)
    {
        const string byCategoryQuery = """
                                       SELECT	p.Id AS PractitionerId,
                                             		anu.FirstName,
                                             		anu.LastName,
                                             		Count( svc.Id ) AS AssignedServiceCalls,
                                             		Count( svc_start.Id ) AS StartedServiceCalls
                                       FROM	dbo.Practitioners AS p
                                       INNER JOIN dbo.AspNetUsers AS anu
                                       ON p.Id = anu.Id
                                       INNER JOIN dbo.PractitionerExpertise AS pe
                                       ON p.Id = pe.PractitionersId
                                       INNER JOIN dbo.Expertises AS e
                                       ON pe.ExpertisesId = e.Id
                                       INNER JOIN dbo.CategoryExpertise AS ce
                                       ON e.Id = ce.AllowedExpertisesId
                                       INNER JOIN dbo.Categories AS c
                                       ON ce.CategoriesId = c.Id
                                       LEFT OUTER JOIN dbo.ServiceCalls AS svc
                                       ON	p.Id = svc.PractitionerId
                                       AND svc.Status IN ( 2, 3 )
                                       LEFT OUTER JOIN dbo.ServiceCalls AS svc_start
                                       ON	p.Id = svc_start.PractitionerId
                                       AND svc.Status = 3
                                       WHERE c.Id = @CategoryId
                                       GROUP BY p.Id, anu.FirstName, anu.LastName
                                       """;

        const string bySubCategoryQuery = """
                           SELECT	p.Id AS PractitionerId,
                           		anu.FirstName,
                           		anu.LastName,
                           		Count( svc.Id ) AS AssignedServiceCalls,
                           		Count( svc_start.Id ) AS StartedServiceCalls
                           FROM	dbo.Practitioners AS p
                           INNER JOIN dbo.AspNetUsers AS anu
                           ON p.Id = anu.Id
                           INNER JOIN dbo.PractitionerExpertise AS pe
                           ON p.Id = pe.PractitionersId
                           INNER JOIN dbo.Expertises AS e
                           ON pe.ExpertisesId = e.Id
                           INNER JOIN dbo.CategoryExpertise AS ce
                           ON e.Id = ce.AllowedExpertisesId
                           INNER JOIN dbo.Categories AS c
                           ON ce.CategoriesId = c.Id
                           INNER JOIN dbo.SubCategories AS sc
                           ON c.Id = sc.CategoryId
                           LEFT OUTER JOIN dbo.ServiceCalls AS svc
                           ON	p.Id = svc.PractitionerId
                           AND svc.Status IN ( 2, 3 )
                           LEFT OUTER JOIN dbo.ServiceCalls AS svc_start
                           ON	p.Id = svc_start.PractitionerId
                           AND svc.Status = 3
                           WHERE sc.Id = @SubCategoryId
                           GROUP BY p.Id, anu.FirstName, anu.LastName
                           """;

        if (request.CategoryId.HasValue)
        {
            var categoryId = CategoryId.From(request.CategoryId.Value);

            return (byCategoryQuery, new { CategoryId = categoryId });
        }

        var subCategoryId = SubCategoryId.From(request.SubCategoryId!.Value);

        return (bySubCategoryQuery, new { SubCategoryId = subCategoryId });
    }

}