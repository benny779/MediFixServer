﻿using FluentEmail.Core;
using MediFix.Application.Abstractions.Email;

namespace MediFix.Infrastructure.Services.Email;

internal class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task<Result> SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var response = await Result.FromTryCatchAsync(() => fluentEmail
            .To(to)
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
        var response = await Result.FromTryCatchAsync(() => fluentEmail
            .To(to)
            .Subject(subject)
            .UsingTemplate(template.GetTemplate(), model)
            .SendAsync(cancellationToken));

        return response;
    }
}
