using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Expertises;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Expertises.UpdateExpertise;

internal sealed class UpdateExpertiseCommandHandler(
    IExpertiseRepository expertiseRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateExpertiseCommand>
{
    public async Task<Result> Handle(UpdateExpertiseCommand request, CancellationToken cancellationToken)
    {
        var expertiseId = ExpertiseId.From(request.ExpertiseId);

        var categoryResult = await expertiseRepository.GetByIdAsync(expertiseId, cancellationToken);

        if (categoryResult.IsFailure)
        {
            return categoryResult.Error;
        }

        if (await expertiseRepository.ExistsAsync(
                c => c.Name == request.Name && c.Id != request.ExpertiseId, cancellationToken))
        {
            return Error.AlreadyExists<Expertise>(nameof(Expertise.Name));
        }

        var expertise = categoryResult.Value;

        expertise.Name = request.Name;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}