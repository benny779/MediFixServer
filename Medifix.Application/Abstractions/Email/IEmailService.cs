using MediFix.SharedKernel.Results;

namespace MediFix.Application.Abstractions.Email;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    Task<Result> SendEmailUsingTemplateAsync<T>(
        string to,
        string subject,
        IEmailTemplate template,
        T model,
        CancellationToken cancellationToken = default);
}