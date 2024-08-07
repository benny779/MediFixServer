using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Expertises.GetExpertises;

internal sealed class GetExpertisesRequestHandler(
    IExpertiseRepository expertiseRepository)
    : IQueryHandler<GetExpertisesRequest, ExpertisesResponse>
{
    public async Task<Result<ExpertisesResponse>> Handle(GetExpertisesRequest request, CancellationToken cancellationToken)
    {
        var expertisesResult = await expertiseRepository.GetAllAsync(cancellationToken);

        if (expertisesResult.IsFailure)
        {
            return expertisesResult.Error;
        }

        return new ExpertisesResponse(expertisesResult
            .Value
            .Select(e => new ExpertiseResponse(e.Id, e.Name)));
    }
}