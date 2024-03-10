using MediFix.SharedKernel.Results;

namespace MediFix.Domain.ServiceCalls;

public static class ServiceCallErrors
{
    public static readonly Error EmptyDetails = Error.Validation(
        "ServiceCall.EmptyDetails",
        "The service call request missing details.");

    public static readonly Error Cancelled = Error.Validation(
        "ServiceCall.Cancelled",
        "The service call is cancelled.");

    public static readonly Error AlreadyCancelled = Error.Validation(
        "ServiceCall.Cancelled",
        "The service call is already cancelled.");

    public static readonly Error CancelledAndCannotBeAssigned = Error.Validation(
        "ServiceCall.Cancelled",
        "The service call is cancelled and cannot be assign to a practitioner.");

    public static readonly Error AlreadyAssigned = Error.Validation(
        "ServiceCall.AlreadyAssign",
        "The service call is already assign to this practitioner.");

    public static readonly Error NotAssigned = Error.Validation(
        "ServiceCall.NotAssigned",
        "The service call is not assigned to a practitioner.");

    public static readonly Error FinishedCannotBeCancelled = Error.Validation(
        "ServiceCall.Finished",
        "The service call is finished and can't be cancelled.");
    
    public static readonly Error FinishedCannotBeFinished = Error.Validation(
        "ServiceCall.Finished",
        "The service call is already finished.");

    public static readonly Error StartedCannotBeAssigned = Error.Validation(
        "ServiceCall.Assigned",
        "The service call is already started and cannot be assigned.");

    public static readonly Error StartedCannotBeCancelled = Error.Validation(
        "ServiceCall.Cancelled",
        "The service call is already started and cannot be cancelled.");

    public static readonly Error StartedCannotStart = Error.Validation(
        "ServiceCall.Started",
        "The service call is already started and cannot be started again.");

    public static readonly Error NotStarted = Error.Validation(
        "ServiceCall.NotStarted",
        "The service call is not started yet.");
}