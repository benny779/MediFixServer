using MediatR;
using MediFix.Application.Categories.AddCategoryExpertise;
using MediFix.Application.Categories.CreateCategory;
using MediFix.Application.Categories.DeleteCategoryExpertise;
using MediFix.Application.Categories.GetCategories;
using MediFix.Application.Categories.GetCategory;
using MediFix.Application.Categories.UpdateCategory;
using MediFix.Application.Expertises.GetExpertiseBy;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class CategoriesController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] bool withInactive, CancellationToken cancellationToken)
    {
        var query = new GetCategoriesRequest(withInactive);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCategoryRequest(id);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.Match(value =>
            CreatedAtAction(nameof(GetById), value),
            Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (IsIdsMismatch(id, request.Id, out var problem))
        {
            return problem!;
        }

        var result = await sender.Send(request, cancellationToken);

        return result.Match(NoContent, Problem);
    }

    [HttpGet("{id:guid}/expertises")]
    public async Task<IActionResult> GetExpertises(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetExpertiseByRequest(CategoryId: id, PractitionerId: null);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpPost("{id:guid}/expertises")]
    public async Task<IActionResult> AddExpertise(Guid id, AddCategoryExpertiseCommand command, CancellationToken cancellationToken)
    {
        if (IsIdsMismatch(id, command.CategoryId, out var problem))
        {
            return problem!;
        }

        var result = await sender.Send(command, cancellationToken);

        return result.Match(NoContent, Problem);
    }

    [HttpDelete("{id:guid}/expertises/{expertiseId:guid}")]
    public async Task<IActionResult> AddExpertise(Guid id, Guid expertiseId, CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryExpertiseCommand(id, expertiseId);

        var result = await sender.Send(command, cancellationToken);

        return result.Match(NoContent, Problem);
    }
}
