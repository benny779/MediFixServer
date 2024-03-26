using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;

namespace MediFix.Application.ServiceCalls.CreateServiceCall;

public record CreateServiceCallCommand(
    UserId UserId,
    LocationId LocationId,
    ServiceCallType ServiceCallType,
    SubCategoryId SubCategoryId,
    string Details,
    ServiceCallPriority Priority = ServiceCallPriority.Low) : ICommand;