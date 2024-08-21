using FluentEmail.Core.Models;

namespace MediFix.Infrastructure.Services.Email;

public static class FluentEmailExtensions
{
    public static Result ToResult(this SendResponse sendResponse)
    {
      if (sendResponse.Successful)
      {
          return Result.Success();
      }

      var errors = string.Join(Environment.NewLine, sendResponse.ErrorMessages);
      return Error.Failure("Email.Failure", errors);
    }
}
