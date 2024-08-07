using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Expertises;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Expertises.GetExpertise;

internal sealed class GetExpertiseRequestHandler(
    IExpertiseRepository expertiseRepository)
    : IQueryHandler<GetExpertiseRequest, ExpertiseResponse>
{
    public async Task<Result<ExpertiseResponse>> Handle(GetExpertiseRequest request, CancellationToken cancellationToken)
    {
        var expertiseId = ExpertiseId.From(request.Id);
        var expertiseResult = await expertiseRepository.GetByIdAsync(expertiseId, cancellationToken);

        if (expertiseResult.IsFailure)
        {
            return expertiseResult.Error;
        }

        return new ExpertiseResponse(expertiseResult.Value.Id, expertiseResult.Value.Name);
    }
}