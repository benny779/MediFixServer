using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Email;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.Application.ServiceCalls.CloseServiceCall;
using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.ServiceCalls.CancelServiceCall;

internal sealed class CancelServiceCallCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IEmailService emailService)
    : ICommandHandler<CancelServiceCallCommand>
{
    public async Task<Result> Handle(CancelServiceCallCommand request, CancellationToken cancellationToken)
    {
        var serviceCallId = ServiceCallId.From(request.Id);

        var serviceCallResult = await serviceCallRepository
            .GetByIdAsync(serviceCallId, cancellationToken);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value;

        var cancelResult = serviceCall.Cancel(currentUser.Id);

        if (cancelResult.IsFailure)
        {
            return cancelResult.Error;
        }

        using var transaction = await unitOfWork.BeginTransactionAsync();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var mailResult = await SendConfirmationEmail(serviceCall, cancellationToken);

        if (mailResult.IsFailure)
        {
            return mailResult.Error;
        }

        transaction.Commit();

        return Result.Success();
    }

    private async Task<Result> SendConfirmationEmail(
        ServiceCall serviceCall,
        CancellationToken cancellationToken)
    {
        var serviceCallResponse = (await serviceCallRepository
            .ToResponse(sc => sc.Id == serviceCall.Id)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken))!;

        var model = new ServiceCallCancelledEmailModel(
            serviceCallResponse.Client.FullName,
            serviceCallResponse.Location.ToPrint(),
            serviceCall.ServiceCallType.ToString(),
            serviceCallResponse.Category.Name,
            serviceCallResponse.SubCategory.Name,
            serviceCallResponse.CurrentStatus.DateTime);

        return await emailService.SendEmailUsingTemplateAsync(
            serviceCallResponse.Client.Email,
            ServiceCallCancelledEmailTemplate.Subject,
            new ServiceCallCancelledEmailTemplate(),
            model,
            cancellationToken);
    }
}