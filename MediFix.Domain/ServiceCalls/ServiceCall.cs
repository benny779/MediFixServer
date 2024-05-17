using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.Users;

namespace MediFix.Domain.ServiceCalls;

public class ServiceCall : AggregateRoot<ServiceCallId>
{
    private readonly List<ServiceCallStatusUpdate> _statusHistory = [];

    public UserId UserId { get; private set; } = null!;

    public LocationId LocationId { get; private set; } = null!;

    public ServiceCallType ServiceCallType { get; private set; }

    public SubCategoryId SubCategoryId { get; private set; } = null!;

    public string Details { get; private set; } = null!;

    public ServiceCallPriority Priority { get; private set; }

    public DateTime DateCreated { get; private set; }

    public ServiceCallStatus Status { get; private set; }

    public DateTime StatusDateTime { get; private set; }

    public PractitionerId? PractitionerId { get; private set; }

    public IReadOnlyCollection<ServiceCallStatusUpdate> StatusHistory => _statusHistory;



    public bool IsAssigned => PractitionerId is not null;
    public bool IsCancelled => Status == ServiceCallStatus.Cancelled;


    private ServiceCall()
    {
    }

    private ServiceCall(ServiceCallId id) : base(id)
    {
    }

    public static Result<ServiceCall> Create(
        UserId userId,
        LocationId locationId,
        ServiceCallType serviceCallType,
        SubCategoryId subCategoryId,
        string details,
        ServiceCallPriority priority = ServiceCallPriority.Low)
    {
        if (string.IsNullOrEmpty(details))
            return ServiceCallErrors.EmptyDetails;

        var serviceCall = new ServiceCall(new ServiceCallId(Guid.NewGuid()))
        {
            UserId = userId,
            LocationId = locationId,
            ServiceCallType = serviceCallType,
            SubCategoryId = subCategoryId,
            Details = details,
            Priority = priority,
            DateCreated = DateTime.Now
        };

        serviceCall.SetStatus(ServiceCallStatus.New, userId);

        // TODO: Service call created event

        return serviceCall;
    }

    private void SetStatus(
        ServiceCallStatus status,
        UserId updateUserId,
        PractitionerId? practitionerId = null)
    {
        Status = status;
        StatusDateTime = DateTime.Now;

        if (practitionerId is not null)
        {
            PractitionerId = practitionerId;
        }

        _statusHistory.Add(new ServiceCallStatusUpdate(Id,
            status,
            StatusDateTime,
            updateUserId,
            practitionerId));
    }

    public Result AssignToPractitioner(UserId updateUserId, PractitionerId practitionerId)
    {
        if (IsCancelled)
            return ServiceCallErrors.CancelledAndCannotBeAssigned;

        if (IsAssigned && practitionerId == PractitionerId)
            return ServiceCallErrors.AlreadyAssigned;

        if (Status == ServiceCallStatus.Started)
            return ServiceCallErrors.StartedCannotBeAssigned;

        PractitionerId = practitionerId;

        SetStatus(ServiceCallStatus.AssignedToPractitioner, updateUserId, practitionerId);

        // TODO: assign to technician event
        // if a practitioner is changed, we need to notify both practitioners

        return Result.Success();
    }

    public Result Cancel(UserId updateUserId)
    {
        if (IsCancelled)
            return ServiceCallErrors.AlreadyCancelled;

        if (Status == ServiceCallStatus.Started)
            return ServiceCallErrors.StartedCannotBeCancelled;

        if (Status == ServiceCallStatus.Finished)
            return ServiceCallErrors.FinishedCannotBeCancelled;

        SetStatus(ServiceCallStatus.Cancelled, updateUserId);

        // TODO: service call cancelled event

        return Result.Success();
    }

    public Result Start(UserId updateUserId)
    {
        if (IsCancelled)
            return ServiceCallErrors.Cancelled;

        if (!IsAssigned)
            return ServiceCallErrors.NotAssigned;

        if (Status == ServiceCallStatus.Started)
            return ServiceCallErrors.StartedCannotStart;

        SetStatus(ServiceCallStatus.Started, updateUserId);

        // TODO: service call Started event

        return Result.Success();
    }

    public Result Finish(UserId updateUserId)
    {
        if (Status != ServiceCallStatus.Started)
            return ServiceCallErrors.NotStarted;

        SetStatus(ServiceCallStatus.Finished, updateUserId);

        // TODO: service call finished event

        return Result.Success();
    }
}