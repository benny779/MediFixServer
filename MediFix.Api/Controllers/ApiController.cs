using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace MediFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected IActionResult Problem(Error error)
    {
        if (error is ValidationError validationError)
        {
            foreach (var e in validationError.Errors)
            {
                ModelState.AddModelError(e.Code, e.Description ?? string.Empty);
            }

            return ValidationProblem();
        }

        if (error.Type == ErrorType.Validation)
        {
            ModelState.AddModelError(error.Code, error.Description ?? string.Empty);
            return ValidationProblem();
        }

        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: error.Code, detail: error.Description);
    }

    public override CreatedAtActionResult CreatedAtAction(string? actionName, object? value)
    {
        if (value is ICreatedResponse created)
        {
            return CreatedAtAction(
                actionName,
                new { created.Id },
                value);
        }

        return base.CreatedAtAction(actionName, value);
    }

    protected bool IsIdsMismatch(Guid id1, Guid id2, out IActionResult? problem)
    {
        if (id1 != id2)
        {
            problem = Problem(Error.Validation(
                "Mismatch.Id",
                $"A mismatch between the IDs ('{id1}' and '{id2}')."));

            return true;
        }

        problem = null;
        return false;
    }
}

