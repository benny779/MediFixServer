using MediFix.Application.Abstractions.Data;
using MediFix.Application.Extensions;
using MediFix.Application.Users;
using MediFix.Application.Users.Entities;
using MediFix.Application.Utils.Persistence;
using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
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
        // Categories
        var categoryPlumbing = new Category(CategoryId.Create(), "Plumbing");
        var categoryAirConditioning = new Category(CategoryId.Create(), "Air conditioning");
        var categoryElectricity = new Category(CategoryId.Create(), "Electricity");


        // Sub categories
        var toilet = new SubCategory(SubCategoryId.Create(), "Toilet", categoryPlumbing.Id);
        var tap = new SubCategory(SubCategoryId.Create(), "Tap", categoryPlumbing.Id);
        var waterBar = new SubCategory(SubCategoryId.Create(), "Water Bar", categoryPlumbing.Id);

        var cool = new SubCategory(SubCategoryId.Create(), "Air conditioner does not cool", categoryAirConditioning.Id);
        var noise = new SubCategory(SubCategoryId.Create(), "Noisy air conditioner", categoryAirConditioning.Id);

        var bulb = new SubCategory(SubCategoryId.Create(), "Bulb replacement", categoryElectricity.Id);
        var sockets = new SubCategory(SubCategoryId.Create(), "No electricity in the sockets", categoryElectricity.Id);


        // Locations
        var buildingA = Location.Create(LocationId.Create(), LocationType.Building, "A").Value!;

        var floor0 = Location.Create(LocationId.Create(), LocationType.Floor, "0", buildingA).Value!;

        var depHr = Location.Create(LocationId.Create(), LocationType.Department, "HR", floor0).Value!;
        var depIt = Location.Create(LocationId.Create(), LocationType.Department, "IT", floor0).Value!;

        var room100 = Location.Create(LocationId.Create(), LocationType.Room, "100", depHr).Value!;
        var room101 = Location.Create(LocationId.Create(), LocationType.Room, "101", depHr).Value!;
        var room102 = Location.Create(LocationId.Create(), LocationType.Room, "102", depHr).Value!;

        var room200 = Location.Create(LocationId.Create(), LocationType.Room, "200", depIt).Value!;
        var room201 = Location.Create(LocationId.Create(), LocationType.Room, "201", depIt).Value!;
        var room202 = Location.Create(LocationId.Create(), LocationType.Room, "202", depIt).Value!;


        // Expertises
        var expertisePlumbing = new Expertise(ExpertiseId.Create(), "Plumbing");
        var expertiseAirConditioning = new Expertise(ExpertiseId.Create(), "Air conditioning");
        var expertiseElectricity = new Expertise(ExpertiseId.Create(), "Electricity");


        // Application users
        var applicationUserClient = GetApplicationUserClient();
        var applicationUserManager = GetApplicationUserManager();
        var applicationUserPlumbing = GetApplicationUserPlumbing();
        var applicationUserAirConditioning = GetApplicationUserAirConditioning();
        var applicationUserElectricity = GetApplicationUserElectricity();


        // Domain users
        var client = Client.Create(ClientId.From(applicationUserClient.Id)).Value!;
        var manager = Manager.Create(ManagerId.From(applicationUserManager.Id)).Value!;
        var practitionerPlumbing = Practitioner.Create(PractitionerId.From(applicationUserPlumbing.Id)).Value!;
        var practitionerAirConditioning = Practitioner.Create(PractitionerId.From(applicationUserAirConditioning.Id)).Value!;
        var practitionerElectricity = Practitioner.Create(PractitionerId.From(applicationUserElectricity.Id)).Value!;

        practitionerPlumbing.AddExpertise(expertisePlumbing);
        practitionerAirConditioning.AddExpertise(expertiseAirConditioning);
        practitionerElectricity.AddExpertise(expertiseElectricity);


        // ServiceCalls
        var serviceCallNew = ServiceCall.Create(client.Id, room100.Id, ServiceCallType.Repair, cool.Id, "Not cool...").Value!;

        var serviceCallFinished = ServiceCall.Create(client.Id, room201.Id, ServiceCallType.Repair, bulb.Id, "It's dark here").Value!;
        serviceCallFinished.AssignToPractitioner(manager.Id, practitionerElectricity.Id);
        serviceCallFinished.Start(practitionerElectricity.Id);
        serviceCallFinished.Finish(practitionerElectricity.Id);

        var serviceCallCancelled = ServiceCall.Create(client.Id, room200.Id, ServiceCallType.New, sockets.Id, "The sockets don't work - no electricity", ServiceCallPriority.High).Value!;
        serviceCallCancelled.Cancel(client.Id);




        // Persist

        using var transaction = await unitOfWork.BeginTransactionAsync();

        context.Categories.AddRange(categoryPlumbing, categoryAirConditioning, categoryElectricity);
        context.SubCategories.AddRange(toilet, tap, waterBar, cool, noise, bulb, sockets);
        context.Locations.AddRange(buildingA, floor0, depHr, depIt, room100, room101, room102, room200, room201, room202);
        context.Expertises.AddRange(expertisePlumbing, expertiseAirConditioning, expertiseElectricity);
        context.Clients.Add(client);
        context.Managers.Add(manager);
        context.Practitioners.AddRange(practitionerPlumbing, practitionerAirConditioning, practitionerElectricity);
        context.ServiceCalls.AddRange(serviceCallNew, serviceCallFinished, serviceCallCancelled);

        var createClientResult = await applicationUserService.CreateAsync(applicationUserClient, DefaultPassword);
        var createManagerResult = await applicationUserService.CreateAsync(applicationUserManager, DefaultPassword);
        var createPlumbingResult = await applicationUserService.CreateAsync(applicationUserPlumbing, DefaultPassword);
        var createAirConditioningResult = await applicationUserService.CreateAsync(applicationUserAirConditioning, DefaultPassword);
        var createElectricityResult = await applicationUserService.CreateAsync(applicationUserElectricity, DefaultPassword);

        List<IdentityResult> createResults =
        [
            createClientResult, createManagerResult, createPlumbingResult, createAirConditioningResult,
                createElectricityResult
        ];

        if (createResults.Any(result => !result.Succeeded))
        {
            transaction.Rollback();

            return createResults.SelectMany(result => result.Errors).ToValidationError();
        }

        await unitOfWork.SaveChangesAsync();

        transaction.Commit();

        return Result.Success();
    }



    private static ApplicationUser GetApplicationUserClient()
    {
        return new ApplicationUser
        {
            Type = UserType.Client,
            Email = "client@medifix.com",
            UserName = "client@medifix.com",
            FirstName = "Client",
            LastName = "Client",
        };
    }

    private static ApplicationUser GetApplicationUserManager()
    {
        return new ApplicationUser
        {
            Type = UserType.Manager,
            Email = "manager@medifix.com",
            UserName = "manager@medifix.com",
            FirstName = "Manager",
            LastName = "Manager",
        };
    }

    private static ApplicationUser GetApplicationUserPlumbing()
    {
        return new ApplicationUser
        {
            Type = UserType.Practitioner,
            Email = "plumbing@medifix.com",
            UserName = "plumbing@medifix.com",
            FirstName = "Practitioner",
            LastName = "Plumbing",
        };
    }

    private static ApplicationUser GetApplicationUserAirConditioning()
    {
        return new ApplicationUser
        {
            Type = UserType.Practitioner,
            Email = "air@medifix.com",
            UserName = "air@medifix.com",
            FirstName = "Practitioner",
            LastName = "Air conditioning",
        };
    }

    private static ApplicationUser GetApplicationUserElectricity()
    {
        return new ApplicationUser
        {
            Type = UserType.Practitioner,
            Email = "electricity@medifix.com",
            UserName = "electricity@medifix.com",
            FirstName = "Practitioner",
            LastName = "Electricity",
        };
    }
}
