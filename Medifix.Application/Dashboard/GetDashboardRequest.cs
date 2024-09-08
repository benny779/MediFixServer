using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Dashboard;

public record GetDashboardRequest(
    DateTime? From,
    DateTime? To) : IQuery<DashboardResponse>
{
    internal bool IsValid => From.HasValue && To.HasValue && From.Value <= To.Value;
}