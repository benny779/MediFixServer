using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.Users;

namespace MediFix.Domain.ServiceCalls;

public class ServiceCall : AggregateRoot<ServiceCallId>
{
    private readonly List<ServiceCallStatusUpdate> _statusHistory = [];

    public ClientId ClientId { get; private set; } = null!;

    public LocationId LocationId { get; private set; } = null!;

    public ServiceCallType ServiceCallType { get; private set; }

    public SubCategoryId SubCategoryId { get; private set; } = null!;

    public ServiceCallPriority Priority { get; private set; }

    public string Details { get; private set; } = null!;

    public DateTime DateCreated { get; private set; }

    public IReadOnlyCollection<ServiceCallStatusUpdate> StatusHistory => _statusHistory;

    public ServiceCallStatus Status { get; private set; }

    public PractitionerId? PractitionerId { get; private set; }

    public ServiceCallStatusUpdate CurrentStatus => _statusHistory.MaxBy(s => s.DateTime)!;
    public bool IsAssigned => CurrentStatus.Status == ServiceCallStatus.AssignedToPractitioner;
    public bool IsCancelled => CurrentStatus.Status == ServiceCallStatus.Cancelled;


    private ServiceCall()
    {
    }

    private ServiceCall(ServiceCallId id) : base(id)
    {
    }

    public static Result<ServiceCall> Create(
        ClientId clientId,
        LocationId locationId,
        ServiceCallType serviceCallType,
        SubCategoryId subCategoryId,
        string details,
        ServiceCallPriority priority = ServiceCallPriority.Low)
    {
        if (string.IsNullOrEmpty(details))
        {
            return ServiceCallErrors.EmptyDetails;
        }

        var serviceCall = new ServiceCall(ServiceCallId.Create())
        {
            ClientId = clientId,
            LocationId = locationId,
            ServiceCallType = serviceCallType,
            SubCategoryId = subCategoryId,
            Details = details,
            Priority = priority,
            DateCreated = DateTime.Now,
            Status = ServiceCallStatus.New
        };

        serviceCall.SetStatus(ServiceCallStatus.New, clientId);

        // TODO: Service call created event

        return serviceCall;
    }

    private void SetStatus(
        ServiceCallStatus status,
        Guid updateUserId,
        PractitionerId? practitionerId = null)
    {
        Status = status;
        PractitionerId = practitionerId ?? PractitionerId;

        _statusHistory.Add(new ServiceCallStatusUpdate(Id,
            status,
            DateTime.Now,
            updateUserId,
            practitionerId));
    }

    public Result AssignToPractitioner(Guid updateUserId, PractitionerId practitionerId)
    {
        if (IsCancelled)
        {
            return ServiceCallErrors.CancelledAndCannotBeAssigned;
        }

        if (IsAssigned && practitionerId == CurrentStatus.PractitionerId)
        {
            return ServiceCallErrors.AlreadyAssigned;
        }

        if (CurrentStatus.Status == ServiceCallStatus.Started)
        {
            return ServiceCallErrors.StartedCannotBeAssigned;
        }

        SetStatus(ServiceCallStatus.AssignedToPractitioner, updateUserId, practitionerId);

        // TODO: assign to technician event
        // if a practitioner is changed, we need to notify both practitioners

        return Result.Success();
    }

    public Result Cancel(Guid updateUserId)
    {
        if (IsCancelled)
        {
            return ServiceCallErrors.AlreadyCancelled;
        }

        if (CurrentStatus.Status == ServiceCallStatus.Started)
        {
            return ServiceCallErrors.StartedCannotBeCancelled;
        }

        if (CurrentStatus.Status == ServiceCallStatus.Finished)
        {
            return ServiceCallErrors.FinishedCannotBeCancelled;
        }

        SetStatus(ServiceCallStatus.Cancelled, updateUserId);

        // TODO: service call cancelled event

        return Result.Success();
    }

    public Result Start(Guid updateUserId)
    {
        if (IsCancelled)
        {
            return ServiceCallErrors.Cancelled;
        }

        if (!IsAssigned)
        {
            return ServiceCallErrors.NotAssigned;
        }

        if (CurrentStatus.Status == ServiceCallStatus.Started)
        {
            return ServiceCallErrors.StartedCannotStart;
        }

        SetStatus(ServiceCallStatus.Started, updateUserId);

        // TODO: service call Started event

        return Result.Success();
    }

    public Result Finish(Guid updateUserId)
    {
        if (CurrentStatus.Status != ServiceCallStatus.Started)
        {
            return ServiceCallErrors.NotStarted;
        }

        SetStatus(ServiceCallStatus.Finished, updateUserId);

        // TODO: service call finished event

        return Result.Success();
    }
}