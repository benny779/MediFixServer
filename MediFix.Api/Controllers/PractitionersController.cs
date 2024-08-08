using MediatR;
using MediFix.Application.Users.Practitioners.AddPractitionerExpertise;
using MediFix.Application.Users.Practitioners.DeletePractitionerExpertise;
using MediFix.Application.Users.Practitioners.GetPractitionerById;
using MediFix.Application.Users.Practitioners.GetPractitioners;
using MediFix.Application.Users.Practitioners.GetPractitionersBySubOrCategory;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class PractitionersController(ISender sender) : ApiController
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPractitionerByIdRequest(id);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Guid? categoryId,
        [FromQuery] Guid? subCategoryId,
        CancellationToken cancellationToken)
    {
        if (categoryId.HasValue || subCategoryId.HasValue)
        {
            var query = new GetPractitionersBySubOrCategoryRequest(categoryId, subCategoryId);

            var result = await sender.Send(query, cancellationToken);

            return result.Match(Ok, Problem);
        }
        else
        {
            var query = new GetPractitionersRequest();

            var result = await sender.Send(query, cancellationToken);

            return result.Match(Ok, Problem);
        }
    }

    [HttpPost("{id:guid}/expertises")]
    public async Task<IActionResult> AddExpertise(Guid id, AddPractitionerExpertiseCommand command, CancellationToken cancellationToken)
    {
        if (IsIdsMismatch(id, command.PractitionerId, out var problem))
        {
            return problem!;
        }

        var result = await sender.Send(command, cancellationToken);

        return result.Match(NoContent, Problem);
    }

    [HttpDelete("{id:guid}/expertises/{expertiseId:guid}")]
    public async Task<IActionResult> AddExpertise(Guid id, Guid expertiseId, CancellationToken cancellationToken)
    {
        var command = new DeletePractitionerExpertiseCommand(id, expertiseId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(NoContent, Problem);
    }
}
