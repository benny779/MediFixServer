using MediatR;
using MediFix.Application.SubCategories.CreateSubCategory;
using MediFix.Application.SubCategories.GetSubCategories;
using MediFix.Application.SubCategories.GetSubCategory;
using MediFix.Application.SubCategories.UpdateSubCategory;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

public class SubCategoriesController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] Guid? categoryId,
        [FromQuery] bool withInactive,
        CancellationToken cancellationToken)
    {
        var query = new GetSubCategoriesRequest(categoryId, withInactive);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetSubCategoryRequest(id);

        var result = await sender.Send(query, cancellationToken);

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        return result.Match(value =>
                CreatedAtAction(nameof(GetById), value),
            Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        if (IsIdsMismatch(id, request.Id, out var problem))
        {
            return problem!;
        }

        var result = await sender.Send(request, cancellationToken);

        return result.Match(NoContent, Problem);
    }
}
