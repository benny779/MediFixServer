using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Expertises;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Expertises.CreateExpertise;

internal sealed class CreateExpertiseCommandHandler(
    IExpertiseRepository expertiseRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateExpertiseCommand, ExpertiseResponse>
{
    public async Task<Result<ExpertiseResponse>> Handle(CreateExpertiseCommand request, CancellationToken cancellationToken)
    {
        if (await expertiseRepository.ExistsAsync(e => e.Name == request.Name, cancellationToken))
        {
            return Error.AlreadyExists<Expertise>(nameof(Expertise.Name));
        }

        var id = ExpertiseId.Create();

        var expertise = new Expertise(id, request.Name);

        expertiseRepository.Insert(expertise);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ExpertiseResponse(expertise.Id, expertise.Name);
    }
}