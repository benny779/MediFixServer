using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.Users.Practitioners.GetPractitionerById;

internal sealed class GetPractitionerByIdRequestHandler(
    IPractitionerRepository practitionerRepository)
    : IQueryHandler<GetPractitionerByIdRequest, PractitionerResponse>
{
    public async Task<Result<PractitionerResponse>> Handle(GetPractitionerByIdRequest request, CancellationToken cancellationToken)
    {
        var practitionerId = PractitionerId.From(request.Id);

        var query = practitionerRepository
            .GetQueryableWithNavigation()
            .Where(p => p.Id == practitionerId);

        var practitioner = await practitionerRepository
            .GetResponseQueryable(query)
            .SingleOrDefaultAsync(cancellationToken);

        if (practitioner is null)
        {
            return Error.EntityNotFound<Practitioner>(request.Id);
        }

        return practitioner;
    }
}