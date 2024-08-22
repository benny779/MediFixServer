using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Email;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.ServiceCalls.CreateServiceCall;

internal sealed class CreateServiceCallCommandHandler(
    IServiceCallRepository serviceCallRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IEmailService emailService)
    : ICommandHandler<CreateServiceCallCommand, CreateServiceCallResponse>
{
    public async Task<Result<CreateServiceCallResponse>> Handle(CreateServiceCallCommand request, CancellationToken cancellationToken)
    {
        var serviceCallResult = ServiceCall.Create(
            ClientId.From(request.ClientId),
            LocationId.From(request.LocationId),
            request.ServiceCallType,
            SubCategoryId.From(request.SubCategoryId),
            request.Details,
            request.Priority);

        if (serviceCallResult.IsFailure)
        {
            return serviceCallResult.Error;
        }

        var serviceCall = serviceCallResult.Value;

        serviceCallRepository.Insert(serviceCall);

        var transaction = await unitOfWork.BeginTransactionAsync();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var mailResult = await emailService.SendEmailAsync(
            currentUser.Email,
            "Your service call was created successfully",
            string.Empty,
            cancellationToken);

        if (mailResult.IsFailure)
        {
            return mailResult.Error;
        }

        transaction.Commit();

        return new CreateServiceCallResponse(serviceCall.Id);
    }
}