using MediFix.Application.Abstractions.Data;
using MediFix.Application.Users.Practitioners;
using MediFix.Domain.Users;

namespace MediFix.Application.Users;

public interface IPractitionerRepository : IRepository<Practitioner, PractitionerId>
{
    IQueryable<PractitionerResponse> GetResponseQueryable();

    IQueryable<PractitionerResponse> GetResponseQueryable(IQueryable<Practitioner> queryable);
}