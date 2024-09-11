using MediFix.Application.Abstractions.Email;
using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Utils.Email.TestEmail;

internal sealed class TestEmailCommandHandler(
    IEmailService emailService)
    : ICommandHandler<TestEmailCommand>
{
    public  Task<Result> Handle(TestEmailCommand request, CancellationToken cancellationToken)
    {
        return emailService.SendEmailAsync(
            request.To,
            request.Subject,
            request.Body,
            cancellationToken);
    }
}