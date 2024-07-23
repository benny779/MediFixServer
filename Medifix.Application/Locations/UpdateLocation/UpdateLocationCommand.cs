using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Locations.UpdateLocation;

public record UpdateLocationCommand(
    Guid LocationId,
    string? Name,
    bool? IsActive) : ICommand;