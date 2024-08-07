namespace MediFix.Application.Expertises;

public record ExpertisesResponse(
    IEnumerable<ExpertiseResponse> Expertises);