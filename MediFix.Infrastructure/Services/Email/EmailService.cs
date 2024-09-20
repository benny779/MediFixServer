using FluentEmail.Core;
using MediFix.Application.Abstractions.Email;
using Microsoft.Extensions.Options;

namespace MediFix.Infrastructure.Services.Email;

internal class EmailService(
    IFluentEmail fluentEmail,
    IOptions<EmailOptions> emailOptions) : IEmailService
{
    private readonly EmailOptions _emailSetting = emailOptions.Value;

    public async Task<Result> SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var recipient = GetRecipient(to);

        var response = await Result.FromTryCatchAsync(() => fluentEmail
            .To(recipient)
            .Subject(subject)
            .Body(body)
            .SendAsync(cancellationToken));

        return response;
    }

    public async Task<Result> SendEmailUsingTemplateAsync<T>(
        string to,
        string subject,
        IEmailTemplate template,
        T model,
        CancellationToken cancellationToken = default)
    {
        var recipient = GetRecipient(to);

        var response = await Result.FromTryCatchAsync(() => fluentEmail
            .To(recipient)
            .Subject(subject)
            .UsingTemplate(template.GetTemplate(), model)
            .SendAsync(cancellationToken));

        return response;
    }

    private string GetRecipient(string to) => _emailSetting.To ?? to;
}