using MediFix.Application.Abstractions.Data;
using MediFix.Domain.Expertises;

namespace MediFix.Application.Expertises;

public interface IExpertiseRepository : IRepository<Expertise, ExpertiseId>;