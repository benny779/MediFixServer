using Bogus;
using MediFix.Application.Abstractions.Data;
using MediFix.Application.Extensions;
using MediFix.Application.Users;
using MediFix.Application.Users.Entities;
using MediFix.Application.Utils.Persistence;
using MediFix.Domain.Categories;
using MediFix.Domain.Expertises;
using MediFix.Domain.Locations;
using MediFix.Domain.Users;
using MediFix.Infrastructure.Services.Fakers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediFix.Infrastructure.Persistence;

internal class PersistenceService(
    IUnitOfWork unitOfWork,
    ApplicationDbContext context,
    IApplicationUserService applicationUserService)
    : IPersistenceService
{
    private const string DefaultPassword = "Aq123456!";
    private const int FakeClientsCount = 50;
    private const int FakeServiceCallsCount = 500;
    private const int FakeServiceCallsStartDateMonths = 3;
    private const int FakeBuildingsCount = 3;
    private const int FakeFloorsCount = 4;
    private const int FakeDepartmentsCount = 4;
    private const int FakeRoomsCount = 10;

    private readonly Faker _faker = new();

    public async Task<Result> ResetDb()
    {
        if (!await context.Database.EnsureDeletedAsync())
        {
            return Error.Failure("ResetDb.Delete.Failed");
        }

        await context.Database.MigrateAsync();

        return Result.Success();
    }

    public async Task<Result> SeedData()
    {
        // Expertises
        var expertiseGeneral = new Expertise(ExpertiseId.Create(), "General");
        var expertisePlumbing = new Expertise(ExpertiseId.Create(), "Plumbing");
        var expertiseAirConditioning = new Expertise(ExpertiseId.Create(), "Air conditioning");
        var expertiseElectricity = new Expertise(ExpertiseId.Create(), "Electricity");
        IList<Expertise> expertises =
            [expertiseGeneral, expertisePlumbing, expertiseAirConditioning, expertiseElectricity];


        // Categories
        var categoryGeneral = new Category(CategoryId.Create(), "General");
        categoryGeneral.AddExpertise(expertiseGeneral);
        var categoryPlumbing = new Category(CategoryId.Create(), "Plumbing");
        categoryPlumbing.AddExpertise(expertisePlumbing);
        var categoryAirConditioning = new Category(CategoryId.Create(), "Air conditioning");
        categoryAirConditioning.AddExpertise(expertiseAirConditioning);
        var categoryElectricity = new Category(CategoryId.Create(), "Electricity");
        categoryElectricity.AddExpertise(expertiseElectricity);
        IList<Category> categories =
            [categoryGeneral, categoryPlumbing, categoryAirConditioning, categoryElectricity];


        // Sub categories
        var general = new SubCategory(SubCategoryId.Create(), "General", categoryGeneral.Id);

        var toilet = new SubCategory(SubCategoryId.Create(), "Toilet", categoryPlumbing.Id);
        var tap = new SubCategory(SubCategoryId.Create(), "Tap", categoryPlumbing.Id);
        var waterBar = new SubCategory(SubCategoryId.Create(), "Water Bar", categoryPlumbing.Id);

        var cool = new SubCategory(SubCategoryId.Create(), "Air conditioner does not cool", categoryAirConditioning.Id);
        var noise = new SubCategory(SubCategoryId.Create(), "Noisy air conditioner", categoryAirConditioning.Id);

        var bulb = new SubCategory(SubCategoryId.Create(), "Bulb replacement", categoryElectricity.Id);
        var sockets = new SubCategory(SubCategoryId.Create(), "No electricity in the sockets", categoryElectricity.Id);

        IList<SubCategory> subCategories =
            [general, toilet, tap, waterBar, cool, noise, bulb, sockets];


        // Locations
        var locations = GetLocations();


        // Application users
        var appUserClient = CreateApplicationUser(UserType.Client, "client");
        var appUserClients = Enumerable.Range(0, FakeClientsCount)
            .Select(x => CreateApplicationUser(UserType.Client, $"{_faker.Internet.UserName()}{x}"))
            .ToList();
        var appUserManager = CreateApplicationUser(UserType.Manager, "manager");
        var appUserPlumbing = CreateApplicationUser(UserType.Practitioner, "plumb");
        var appUserPlumbing2 = CreateApplicationUser(UserType.Practitioner, "plumb2");
        var appUserAirConditioning = CreateApplicationUser(UserType.Practitioner, "air");
        var appUserAirConditioning2 = CreateApplicationUser(UserType.Practitioner, "air2");
        var appUserElectricity = CreateApplicationUser(UserType.Practitioner, "elec");
        var appUserElectricity2 = CreateApplicationUser(UserType.Practitioner, "elec2");
        IList<ApplicationUser> applicationUsers =
        [
            appUserClient, .. appUserClients, appUserManager, appUserPlumbing, appUserPlumbing2,
            appUserAirConditioning, appUserAirConditioning2, appUserElectricity, appUserElectricity2
        ];


        // Domain users
        var client = Client.Create(ClientId.From(appUserClient.Id)).Value;
        var fakeClients = appUserClients.Select(c => Client.Create(ClientId.From(c.Id)).Value);
        var manager = Manager.Create(ManagerId.From(appUserManager.Id)).Value;
        var practitionerPlumbing = Practitioner.Create(PractitionerId.From(appUserPlumbing.Id)).Value;
        var practitionerPlumbing2 = Practitioner.Create(PractitionerId.From(appUserPlumbing2.Id)).Value;
        var practitionerAirConditioning = Practitioner.Create(PractitionerId.From(appUserAirConditioning.Id)).Value;
        var practitionerAirConditioning2 = Practitioner.Create(PractitionerId.From(appUserAirConditioning2.Id)).Value;
        var practitionerElectricity = Practitioner.Create(PractitionerId.From(appUserElectricity.Id)).Value;
        var practitionerElectricity2 = Practitioner.Create(PractitionerId.From(appUserElectricity2.Id)).Value;

        practitionerPlumbing.AddExpertise(expertisePlumbing);
        practitionerPlumbing2.AddExpertise(expertisePlumbing);
        practitionerAirConditioning.AddExpertise(expertiseAirConditioning);
        practitionerAirConditioning2.AddExpertise(expertiseAirConditioning);
        practitionerElectricity.AddExpertise(expertiseElectricity);
        practitionerElectricity2.AddExpertise(expertiseElectricity);

        IList<Client> clients = [client, .. fakeClients];
        IList<Practitioner> practitioners =
        [
            practitionerPlumbing, practitionerPlumbing2, practitionerAirConditioning,
            practitionerAirConditioning2, practitionerElectricity, practitionerElectricity2
        ];
        foreach (var practitioner in practitioners)
        {
            practitioner.AddExpertise(expertiseGeneral);
        }


        // ServiceCalls
        var rooms = locations
            .Where(location => location.LocationType == LocationType.Room)
            .ToArray();

        var serviceCallFaker = new ServiceCallFakerFactory(
            DateTime.Now.AddMonths(-FakeServiceCallsStartDateMonths),
            DateTime.Now,
            clients,
            rooms,
            categories,
            subCategories,
            practitioners,
            manager.Id);

        var serviceCalls = serviceCallFaker.Generate(FakeServiceCallsCount);


        // Persist
        using var transaction = await unitOfWork.BeginTransactionAsync();

        context.Categories.AddRange(categories);
        context.SubCategories.AddRange(subCategories);
        context.Locations.AddRange(locations);
        context.Expertises.AddRange(expertises);
        context.Clients.AddRange(clients);
        context.Managers.Add(manager);
        context.Practitioners.AddRange(practitioners);
        context.ServiceCalls.AddRange(serviceCalls);


        List<IdentityResult> createResults = [];

        foreach (var applicationUser in applicationUsers)
        {
            createResults.Add(await applicationUserService.CreateAsync(applicationUser, DefaultPassword));
        }

        if (createResults.Any(result => !result.Succeeded))
        {
            transaction.Rollback();

            return createResults.SelectMany(result => result.Errors).ToValidationError();
        }

        await unitOfWork.SaveChangesAsync();

        transaction.Commit();

        return Result.Success();
    }



    private static IEnumerable<Location> CreateLocations(Location? parent, LocationType type, int first, int count)
    {
        return Enumerable.Range(first, count)
             .Select(x => Location.Create(LocationId.Create(), type, $"{type} {x}", parent).Value);
    }

    private static List<Location> GetLocations()
    {
        var buildings = CreateLocations(null, LocationType.Building, 1, FakeBuildingsCount)
            .ToList();

        var floors = buildings
            .SelectMany(building => CreateLocations(building, LocationType.Floor, ExtractNumbers(building.Name), FakeFloorsCount))
            .ToList();

        var departments = floors
            .SelectMany(floor => CreateLocations(floor, LocationType.Department, ExtractNumbers(floor.Name), FakeDepartmentsCount))
            .ToList();

        var rooms = departments
            .SelectMany(dep => CreateLocations(dep, LocationType.Room, ExtractNumbers(dep.Name) * 2 + 100, FakeRoomsCount))
            .ToList();


        return [.. buildings, .. floors, .. departments, .. rooms];
    }

    private ApplicationUser CreateApplicationUser(UserType userType, string userName)
    {
        var email = $"{userName}@medifix.com";

        return new ApplicationUser
        {
            Type = userType,
            Email = email,
            UserName = email,
            FirstName = _faker.Name.FirstName(),
            LastName = _faker.Name.LastName()
        };
    }

    private static int ExtractNumbers(string str)
    {
        var digits = str.Where(char.IsDigit).ToArray();

        return digits.Length > 0
            ? int.Parse(new string(digits))
            : throw new InvalidOperationException();
    }
}