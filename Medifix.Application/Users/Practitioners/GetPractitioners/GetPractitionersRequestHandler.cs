using MediFix.Application.Abstractions.Messaging;
using MediFix.Application.Expertises;
using MediFix.SharedKernel.Extensions;
using MediFix.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Application.Users.Practitioners.GetPractitioners;

internal sealed class GetPractitionersRequestHandler(
    IPractitionerRepository practitionerRepository,
    IApplicationUserRepository applicationUserRepository)
    : IQueryHandler<GetPractitionersRequest, PractitionersResponse>
{
    public async Task<Result<PractitionersResponse>> Handle(GetPractitionersRequest request, CancellationToken cancellationToken)
    {
        var practitioners = await applicationUserRepository
            .GetQueryable()
            .AsNoTracking()
            .Join(practitionerRepository
                    .GetQueryableWithNavigation()
                    .AsNoTracking(),
                appUser => appUser.Id,
                practitioner => practitioner.Id,
                (appUser, practitioner) => new PractitionerResponse(
                    practitioner.Id,
                    appUser.FirstName,
                    appUser.LastName,
                    appUser.FullName,
                    practitioner.Expertises.Select(exp => new ExpertiseResponse(exp.Id, exp.Name)))
            ).ToListAsync(cancellationToken);

        return practitioners.IsEmpty()
            ? new PractitionersResponse(Enumerable.Empty<PractitionerResponse>())
            : new PractitionersResponse(practitioners);
    }
}