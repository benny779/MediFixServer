using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Email;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.Domain.ServiceCalls;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.ServiceCalls.CloseServiceCall;

internal sealed class CloseServiceCallCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IEmailService emailService)
    : ICommandHandler<CloseServiceCallCommand>
{
    public async Task<Result> Handle(CloseServiceCallCommand request, CancellationToken cancellationToken)
    {
        var serviceCallId = ServiceCallId.From(request.ServiceCallId);

        var serviceCallResult = await serviceCallRepository.GetByIdAsync(serviceCallId, cancellationToken);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value;
        
        var qrGuid = Guid.Parse(request.QrCode);
        if (serviceCall.LocationId.Value != qrGuid)
        {
            return Error.Validation(
                "ServiceCall.Finish.QrCode",
                "Error validating the QR code.");
        }

        var finishResult = serviceCall.Finish(currentUser.Id, request.CloseDetails);

        if (finishResult.IsFailure)
        {
            return finishResult.Error;
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

        var model = new ServiceCallClosedEmailModel(
            serviceCallResponse.Client.FullName,
            serviceCallResponse.Location.ToPrint(),
            serviceCall.ServiceCallType.ToString(),
            serviceCallResponse.Category.Name,
            serviceCallResponse.SubCategory.Name,
            serviceCallResponse.Practitioner!.FullName,
            serviceCallResponse.CurrentStatus.DateTime,
            serviceCall.CloseDetails!);

        return await emailService.SendEmailUsingTemplateAsync(
            serviceCallResponse.Client.Email,
            ServiceCallClosedEmailTemplate.Subject,
            new ServiceCallClosedEmailTemplate(),
            model,
            cancellationToken);
    }
}