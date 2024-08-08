using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Expertises;
using MediFix.Domain.Expertises;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Users.Practitioners.DeletePractitionerExpertise;

internal sealed class DeletePractitionerExpertiseCommandHandler(
    IPractitionerRepository practitionerRepository,
    IExpertiseRepository expertiseRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeletePractitionerExpertiseCommand>
{
    public async Task<Result> Handle(DeletePractitionerExpertiseCommand request, CancellationToken cancellationToken)
    {
        var practitionerId = PractitionerId.From(request.PractitionerId);
        var expertiseId = ExpertiseId.From(request.ExpertiseId);

        var practitionerResult = await practitionerRepository
            .GetByIdWithNavigationAsync(practitionerId, cancellationToken);

        if (practitionerResult.IsFailure)
        {
            return practitionerResult.Error;
        }

        var expertiseResult = await expertiseRepository
            .GetByIdWithNavigationAsync(expertiseId, cancellationToken);

        if (expertiseResult.IsFailure)
        {
            return expertiseResult.Error;
        }

        if (practitionerResult.Value.RemoveExpertise(expertiseResult.Value))
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}