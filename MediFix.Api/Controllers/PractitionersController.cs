using MediatR;
using MediFix.Application.Users.Practitioners.GetPractitionersBySubOrCategory;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class PractitionersController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetBySubAndCategory(
        [FromQuery] Guid? categoryId,
        [FromQuery] Guid? subCategoryId,
        CancellationToken cancellationToken)
    {
        var query = new GetPractitionersBySubOrCategoryRequest(categoryId, subCategoryId);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }
}
