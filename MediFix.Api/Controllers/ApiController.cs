﻿using MediFix.SharedKernel.Results;
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
}
