using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Email;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Abstractions.Services;
using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

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

        var mailResult = await SendConfirmationEmail(serviceCall, cancellationToken);

        if (mailResult.IsFailure)
        {
            return mailResult.Error;
        }

        transaction.Commit();

        return new CreateServiceCallResponse(serviceCall.Id);
    }

    private async Task<Result> SendConfirmationEmail(
        ServiceCall serviceCall,
        CancellationToken cancellationToken)
    {
        var serviceCallResponse = (await serviceCallRepository
            .ToResponse(sc => sc.Id == serviceCall.Id)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken))!;

        var model = new ServiceCallCreatedEmailModel(
            serviceCallResponse.Client.FullName,
            serviceCallResponse.Location.ToPrint(),
            serviceCall.ServiceCallType.ToString(),
            serviceCallResponse.Category.Name, 
            serviceCallResponse.SubCategory.Name, 
            serviceCall.Priority.ToString(), 
            serviceCall.Details);

        return await emailService.SendEmailUsingTemplateAsync(
            serviceCallResponse.Client.Email,
            "Service Call Confirmation",
            new ServiceCallCreatedEmailTemplate(),
            model,
            cancellationToken);
    }
}