using MediatR;
using MediFix.Application.Categories.GetCategories;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class CategoriesController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var query = new GetCategoriesRequest();

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }
}
