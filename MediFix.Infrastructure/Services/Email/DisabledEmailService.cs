using MediFix.Application.Abstractions.Email;

namespace MediFix.Infrastructure.Services.Email;

internal class DisabledEmailService : IEmailService
{
    public Task<Result> SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        return Result.Success().AsTask();
    }

    public Task<Result> SendEmailUsingTemplateAsync<T>(string to, string subject, IEmailTemplate template, T model,
        CancellationToken cancellationToken = default)
    {
        return Result.Success().AsTask();
    }
}