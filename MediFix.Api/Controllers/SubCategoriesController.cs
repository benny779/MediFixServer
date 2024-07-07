using MediatR;
using MediFix.Application.SubCategories.GetSubCategoriesByCategory;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class SubCategoriesController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid categoryId, CancellationToken cancellationToken)
    {
        var query = new GetSubCategoriesByCategoryRequest(categoryId);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }
}
