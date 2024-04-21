using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;
using MediFix.SharedKernel.Results;

namespace MediFix.Application.Locations.GetLocationTypes;

internal sealed class GetLocationTypesRequestHandler : IQueryHandler<
    GetLocationTypesRequest, GetLocationTypesResponse>
{
    public Task<Result<GetLocationTypesResponse>> Handle(
        GetLocationTypesRequest request,
        CancellationToken cancellationToken)
    {
        var locationTypes = Enum
            .GetValues<LocationType>()
            .ToList();

        var response = new GetLocationTypesResponse(locationTypes);

        return Result.Success(response).AsTask();
        //return Task.FromResult(Result.Success(response));
    }
}