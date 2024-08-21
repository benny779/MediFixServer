using MediFix.SharedKernel.Results;

namespace MediFix.Application.Abstractions.Email;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
