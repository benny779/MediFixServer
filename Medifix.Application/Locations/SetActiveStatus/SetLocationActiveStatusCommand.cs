using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.SetActiveStatus;

public record SetLocationActiveStatusCommand(LocationId LocationId, bool IsActive) : ICommand;