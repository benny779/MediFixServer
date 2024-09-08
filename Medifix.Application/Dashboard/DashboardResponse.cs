using MediFix.Domain.ServiceCalls;

namespace MediFix.Application.Dashboard;

public record DashboardResponse(
    DashboardCountBoxes CountBoxes,
    IEnumerable<DashboardPractitioner> Practitioners,
    IEnumerable<DashboardNameValue> PriorityPieData,
    IEnumerable<DashboardNameValue> CategoryPieData,
    IEnumerable<DashboardNameValue> TypePieData,
    IEnumerable<DashboardBuilding> Buildings);

public record ServiceCallsCountByStatus
{
    private ServiceCallsCountByStatus() { }
    public ServiceCallsCountByStatus(ServiceCallStatus Status, int Count)
    {
        this.Status = Status;
        this.Count = Count;
    }

    public ServiceCallStatus Status { get; init; }
    public int Count { get; init; }
}

public record ServiceCallsCountByPriority
{
    private ServiceCallsCountByPriority() { }
    public ServiceCallsCountByPriority(ServiceCallPriority Priority, int Count)
    {
        this.Priority = Priority;
        this.Count = Count;
    }

    public ServiceCallPriority Priority { get; init; }
    public int Count { get; init; }
}

public record ServiceCallsCountByType
{
    private ServiceCallsCountByType() { }
    public ServiceCallsCountByType(ServiceCallType Type, int Count)
    {
        this.Type = Type;
        this.Count = Count;
    }

    public ServiceCallType Type { get; init; }
    public int Count { get; init; }
}

public record ServiceCallsCountByCategory(string CategoryName, int Count);

public record Practitioner(
    Guid PractitionerId,
    string FirstName,
    string LastName,
    int Assigned,
    int Started,
    int Finished,
    int? AvgDurationMinutes);

public record Building(
    string BuildingName,
    int Total,
    int Open,
    int Assigned,
    int Started,
    int Finished,
    int Cancelled,
    int? AvgDurationMinutes);



public record DashboardCountBoxes(
    int TotalCreated,
    int Open,
    int Assigned,
    int Started,
    int Finished)
{
    public static DashboardCountBoxes FromServiceCallsCountByStatus(IEnumerable<ServiceCallsCountByStatus> countByStatus)
    {
        var statusEnumerable = countByStatus as ServiceCallsCountByStatus[] ?? countByStatus.ToArray();

        return new DashboardCountBoxes(
            statusEnumerable.Sum(x => x.Count),
            statusEnumerable.Single(x => x.Status == ServiceCallStatus.New).Count,
            statusEnumerable.Single(x => x.Status == ServiceCallStatus.AssignedToPractitioner).Count,
            statusEnumerable.Single(x => x.Status == ServiceCallStatus.Started).Count,
            statusEnumerable.Single(x => x.Status == ServiceCallStatus.Finished).Count
        );
    }
}

public record DashboardPractitioner(
    string Name,
    int Assigned,
    int Started,
    int Finished,
    int? AvgDurationMinutes)
{
    public static DashboardPractitioner FromPractitioner(Practitioner practitioner)
    {
        return new DashboardPractitioner(
            $"{practitioner.FirstName} {practitioner.LastName}",
            practitioner.Assigned,
            practitioner.Started,
            practitioner.Finished,
            practitioner.AvgDurationMinutes);
    }
}

public record DashboardNameValue(string Name, int Value);

public record DashboardBuilding(
    string Name,
    int Open,
    int Assigned,
    int Started,
    int Finished,
    int TotalServiceCalls,
    int? AvgDurationMinutes)
{
    public static DashboardBuilding FromBuilding(Building building)
    {
        return new DashboardBuilding(
            building.BuildingName,
            building.Open,
            building.Assigned,
            building.Started,
            building.Finished,
            building.Open + building.Assigned + building.Started + building.Finished,
            building.AvgDurationMinutes);
    }
}