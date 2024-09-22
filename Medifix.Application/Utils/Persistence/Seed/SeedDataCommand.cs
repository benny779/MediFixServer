using MediFix.Application.Abstractions.Messaging;

namespace MediFix.Application.Utils.Persistence.Seed;

public record SeedDataCommand(
    int FakeClientsCount = 50,
    int FakeBuildingsCount = 3,
    int FakeFloorsCount = 4,
    int FakeDepartmentsCount = 4,
    int FakeRoomsCount = 10,
    int FakeServiceCallsCount = 500,
    int FakeServiceCallsStartDateMonths = 3,
    string DefaultPassword = "Aq123456!") : ICommand;