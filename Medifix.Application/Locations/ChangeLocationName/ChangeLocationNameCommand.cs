using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.ChangeLocationName;

public record ChangeLocationNameCommand(
    LocationId LocationId,
    string Name) : ICommand;