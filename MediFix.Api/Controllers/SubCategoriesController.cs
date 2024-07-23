using MediatR;
using MediFix.Application.SubCategories.GetSubCategoriesByCategory;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class SubCategoriesController(ISender sender) : ApiController
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetSubCategoriesByCategoryRequest(id);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }
}
