using MediFix.Application.Abstractions.Messaging;
using MediFix.Domain.Locations;

namespace MediFix.Application.Locations.DeleteLocation;

public record DeleteLocationCommand(LocationId LocationId) : ICommand;