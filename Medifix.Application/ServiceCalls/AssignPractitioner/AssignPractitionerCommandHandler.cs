﻿using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.SubCategories;
using MediFix.Application.Users;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.AssignPractitioner;

internal sealed class AssignPractitionerCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IPractitionerRepository practitionerRepository,
    ISubCategoryRepository subCategoryRepository,
    IUnitOfWork unitOfWork)
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


        // TODO: Get current user
        var currentUserId = Guid.NewGuid();

        var assignResult = serviceCall.AssignPractitioner(currentUserId, practitionerId);

        if (assignResult.IsFailure)
        {
            return assignResult.Error;
        }

        serviceCallRepository.Update(serviceCall);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}