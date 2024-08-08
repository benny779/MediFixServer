using MediFix.Application.Abstractions.Messaging;
using MediFix.SharedKernel.Extensions;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.Users.Practitioners.GetPractitioners;

internal sealed class GetPractitionersRequestHandler(
    IPractitionerRepository practitionerRepository)
    : IQueryHandler<GetPractitionersRequest, PractitionersResponse>
{
    public async Task<Result<PractitionersResponse>> Handle(GetPractitionersRequest request, CancellationToken cancellationToken)
    {
        var practitioners = await practitionerRepository
            .GetResponseQueryable()
            .ToListAsync(cancellationToken);

        return practitioners.IsEmpty()
            ? new PractitionersResponse(Enumerable.Empty<PractitionerResponse>())
            : new PractitionersResponse(practitioners);
    }
}