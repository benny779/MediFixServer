using MediFix.Domain.Categories;
using MediFix.Domain.Core.Primitives;
using MediFix.Domain.Users;
using MediFix.SharedKernel.Results;

namespace MediFix.Domain.ServiceCalls;

public class ServiceCall : Entity<ServiceCallId>
{
    private readonly List<ServiceCallStatusUpdate> _statusUpdates = [];

    public UserId UserId { get; private set; }

    public LocationId LocationId { get; private set; }

    public SubCategoryId SubCategoryId { get; private set; }

    public string Details { get; private set; }

    public ServiceCallPriority Priority { get; private set; }

    public DateTime DateCreated { get; private set; }

    public IReadOnlyCollection<ServiceCallStatusUpdate> Statuses => _statusUpdates;

    public TechnicianId? TechnicianId { get; private set; }


    public bool IsAssigned => TechnicianId is not null;
    public bool IsCancelled => CurrentStatus == ServiceCallStatus.Cancelled;
    public ServiceCallStatus CurrentStatus => Statuses.MaxBy(s => s.DateTime)!.Status;


    private ServiceCall(ServiceCallId id) : base(id)
    {
    }

    public static Result<ServiceCall> Create(
        UserId userId,
        LocationId locationId,
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
            SubCategoryId = subCategoryId,
            Details = details,
            Priority = priority,
            DateCreated = DateTime.Now
        };

        serviceCall.SetStatus(ServiceCallStatus.New);

        // TODO: Service call created event

        return serviceCall;
    }

    private void SetStatus(ServiceCallStatus status) =>
        _statusUpdates.Add(new ServiceCallStatusUpdate(status, DateTime.Now));

    public Result AssignToTechnician(TechnicianId technicianId)
    {
        if (IsCancelled)
            return ServiceCallErrors.CancelledAndCannotBeAssigned;

        if (IsAssigned)
            return ServiceCallErrors.AlreadyAssigned;

        TechnicianId = technicianId;

        SetStatus(ServiceCallStatus.AssignedToTechnician);

        // TODO: assign to technician event

        return Result.Success();
    }

    public Result Cancel()
    {
        if (IsCancelled)
            return ServiceCallErrors.AlreadyCancelled;

        if (CurrentStatus == ServiceCallStatus.Finished)
            return ServiceCallErrors.FinishedCannotBeCancelled;

        SetStatus(ServiceCallStatus.Cancelled);

        // TODO: service call cancelled event

        return Result.Success();
    }

    public Result TechnicianStarted()
    {
        if (IsCancelled)
            return ServiceCallErrors.Cancelled;

        SetStatus(ServiceCallStatus.TechnicianStarted);

        // TODO: service call TechnicianStarted event

        return Result.Success();
    }

    public Result Finish()
    {
        if (IsCancelled)
            return ServiceCallErrors.Cancelled;

        if (CurrentStatus == ServiceCallStatus.Finished)
            return ServiceCallErrors.FinishedCannotBeFinished;

        SetStatus(ServiceCallStatus.Finished);

        // TODO: service call finished event

        return Result.Success();
    }
}