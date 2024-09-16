using MediFix.Application.Abstractions.Email;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.ServiceCalls.CreateServiceCall;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Utils.Email.TestEmailTemplate;

public record TestEmailTemplateCommand(string To) : ICommand;

internal sealed class TestEmailTemplateCommandHandler(
    IEmailService emailService)
    : ICommandHandler<TestEmailTemplateCommand>
{
    public Task<Result> Handle(TestEmailTemplateCommand request, CancellationToken cancellationToken)
    {
        var model = new ServiceCallCreatedEmailModel(
            "Israel Israeli",
            "Location Details",
            "New",
            "Some Category",
            "Some Subcategory",
            "High",
            "Details text...");

        return emailService.SendEmailUsingTemplateAsync(
            request.To,
            "Template email test",
            new ServiceCallCreatedEmailTemplate(),
            model,
            cancellationToken);
    }
}