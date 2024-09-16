using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Email;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.Application.ServiceCalls.CreateServiceCall;
using MediFix.Application.SubCategories;
using MediFix.Application.Users;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.ServiceCalls.AssignPractitioner;

internal sealed class AssignPractitionerCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IPractitionerRepository practitionerRepository,
    ISubCategoryRepository subCategoryRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IEmailService emailService)
    : ICommandHandler<AssignPractitionerCommand>
{
    public async Task<Result> Handle(AssignPractitionerCommand request, CancellationToken cancellationToken)
    {
        var serviceCallId = ServiceCallId.From(request.ServiceCallId);

        var serviceCallResult = await serviceCallRepository
            .GetByIdAsync(serviceCallId, cancellationToken);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value;


        var practitionerId = PractitionerId.From(request.PractitionerId);

        var practitionerResult = await practitionerRepository
            .GetByIdWithNavigationAsync(practitionerId, cancellationToken);

        if (practitionerResult.IsFailure)
        {
            return practitionerResult.Error;
        }

        var practitioner = practitionerResult.Value;

        var subCategoryResult = await subCategoryRepository
            .GetByIdWithNavigationAsync(serviceCall.SubCategoryId, cancellationToken);

        if (subCategoryResult.IsFailure)
        {
            return subCategoryResult.Error;
        }

        var category = subCategoryResult.Value.Category;


        if (!category.IsPractitionerAllowed(practitioner))
        {
            return Error.Validation(
                "ServiceCall.Assign.NotAllowed",
                "This practitioner is not allowed to be assigned to this service call.");
        }

        var assignResult = serviceCall.AssignPractitioner(currentUser.Id, practitionerId);

        if (assignResult.IsFailure)
        {
            return assignResult.Error;
        }

        using var transaction = await unitOfWork.BeginTransactionAsync();

        serviceCallRepository.Update(serviceCall);

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

        var model = new PractitionerAssignedEmailModel(
            serviceCallResponse.Practitioner!.FullName,
            serviceCallResponse.Client.FullName,
            serviceCallResponse.Location.ToPrint(),
            serviceCall.ServiceCallType.ToString(),
            serviceCallResponse.Category.Name,
            serviceCallResponse.SubCategory.Name,
            serviceCall.Priority.ToString(),
            serviceCall.DateCreated,
            serviceCall.Details);

        return await emailService.SendEmailUsingTemplateAsync(
            serviceCallResponse.Practitioner.Email,
            "New Service Call Assignment",
            new PractitionerAssignedEmailTemplate(),
            model,
            cancellationToken);
    }
}