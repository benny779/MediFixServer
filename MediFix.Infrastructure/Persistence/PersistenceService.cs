﻿using MediFix.Application.Abstractions.Data;
using MediFix.Application.Extensions;
using MediFix.Application.Users;
using MediFix.Application.Users.Entities;
using MediFix.Application.Utils.Persistence;
using MediFix.Domain.Categories;
using MediFix.Domain.Expertises;
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
        // Expertises
        var expertisePlumbing = new Expertise(ExpertiseId.Create(), "Plumbing");
        var expertiseAirConditioning = new Expertise(ExpertiseId.Create(), "Air conditioning");
        var expertiseElectricity = new Expertise(ExpertiseId.Create(), "Electricity");


        // Categories
        var categoryPlumbing = new Category(CategoryId.Create(), "Plumbing");
        var categoryAirConditioning = new Category(CategoryId.Create(), "Air conditioning");
        var categoryElectricity = new Category(CategoryId.Create(), "Electricity");

        categoryPlumbing.AddExpertise(expertisePlumbing);
        categoryAirConditioning.AddExpertise(expertiseAirConditioning);
        categoryElectricity.AddExpertise(expertiseElectricity);


        // Sub categories
        var toilet = new SubCategory(SubCategoryId.Create(), "Toilet", categoryPlumbing.Id);
        var tap = new SubCategory(SubCategoryId.Create(), "Tap", categoryPlumbing.Id);
        var waterBar = new SubCategory(SubCategoryId.Create(), "Water Bar", categoryPlumbing.Id);

        var cool = new SubCategory(SubCategoryId.Create(), "Air conditioner does not cool", categoryAirConditioning.Id);
        var noise = new SubCategory(SubCategoryId.Create(), "Noisy air conditioner", categoryAirConditioning.Id);

        var bulb = new SubCategory(SubCategoryId.Create(), "Bulb replacement", categoryElectricity.Id);
        var sockets = new SubCategory(SubCategoryId.Create(), "No electricity in the sockets", categoryElectricity.Id);


        // Locations
        var locations = GetLocations();


        // Application users
        var applicationUserClient = GetApplicationUserClient();
        var applicationUserManager = GetApplicationUserManager();
        var applicationUserPlumbing = GetApplicationUserPlumbing();
        var applicationUserPlumbing2 = GetApplicationUserPlumbing2();
        var applicationUserAirConditioning = GetApplicationUserAirConditioning();
        var applicationUserAirConditioning2 = GetApplicationUserAirConditioning2();
        var applicationUserElectricity = GetApplicationUserElectricity();
        var applicationUserElectricity2 = GetApplicationUserElectricity2();


        // Domain users
        var client = Client.Create(ClientId.From(applicationUserClient.Id)).Value;
        var manager = Manager.Create(ManagerId.From(applicationUserManager.Id)).Value;
        var practitionerPlumbing = Practitioner.Create(PractitionerId.From(applicationUserPlumbing.Id)).Value;
        var practitionerPlumbing2 = Practitioner.Create(PractitionerId.From(applicationUserPlumbing2.Id)).Value;
        var practitionerAirConditioning = Practitioner.Create(PractitionerId.From(applicationUserAirConditioning.Id)).Value;
        var practitionerAirConditioning2 = Practitioner.Create(PractitionerId.From(applicationUserAirConditioning2.Id)).Value;
        var practitionerElectricity = Practitioner.Create(PractitionerId.From(applicationUserElectricity.Id)).Value;
        var practitionerElectricity2 = Practitioner.Create(PractitionerId.From(applicationUserElectricity2.Id)).Value;

        practitionerPlumbing.AddExpertise(expertisePlumbing);
        practitionerPlumbing2.AddExpertise(expertisePlumbing);
        practitionerAirConditioning.AddExpertise(expertiseAirConditioning);
        practitionerAirConditioning2.AddExpertise(expertiseAirConditioning);
        practitionerElectricity.AddExpertise(expertiseElectricity);
        practitionerElectricity2.AddExpertise(expertiseElectricity);


        // ServiceCalls
        var allRooms = locations
            .Where(location => location.LocationType == LocationType.Room)
            .ToArray();

        Random.Shared.Shuffle(allRooms);

        var rooms = allRooms.Take(3).ToList();

        var serviceCallNew = ServiceCall.Create(client.Id, rooms[0].Id, ServiceCallType.Repair, cool.Id, "Not cool...").Value;

        var serviceCallFinished = ServiceCall.Create(client.Id, rooms[1].Id, ServiceCallType.Repair, bulb.Id, "It's dark here").Value;
        serviceCallFinished.AssignPractitioner(manager.Id, practitionerElectricity.Id);
        serviceCallFinished.Start(practitionerElectricity.Id);
        serviceCallFinished.Finish(practitionerElectricity.Id);

        var serviceCallCancelled = ServiceCall.Create(client.Id, rooms[2].Id, ServiceCallType.New, sockets.Id, "The sockets don't work - no electricity", ServiceCallPriority.High).Value;
        serviceCallCancelled.Cancel(client.Id);



        // Persist
        using var transaction = await unitOfWork.BeginTransactionAsync();

        context.Categories.AddRange(categoryPlumbing, categoryAirConditioning, categoryElectricity);
        context.SubCategories.AddRange(toilet, tap, waterBar, cool, noise, bulb, sockets);
        context.Locations.AddRange(locations);
        context.Expertises.AddRange(expertisePlumbing, expertiseAirConditioning, expertiseElectricity);
        context.Clients.Add(client);
        context.Managers.Add(manager);
        context.Practitioners.AddRange(
            practitionerPlumbing,
            practitionerPlumbing2,
            practitionerAirConditioning,
            practitionerAirConditioning2,
            practitionerElectricity,
            practitionerElectricity2);
        context.ServiceCalls.AddRange(serviceCallNew, serviceCallFinished, serviceCallCancelled);

        var createClientResult = await applicationUserService.CreateAsync(applicationUserClient, DefaultPassword);
        var createManagerResult = await applicationUserService.CreateAsync(applicationUserManager, DefaultPassword);
        var createPlumbingResult = await applicationUserService.CreateAsync(applicationUserPlumbing, DefaultPassword);
        var createPlumbing2Result = await applicationUserService.CreateAsync(applicationUserPlumbing2, DefaultPassword);
        var createAirConditioningResult = await applicationUserService.CreateAsync(applicationUserAirConditioning, DefaultPassword);
        var createAirConditioning2Result = await applicationUserService.CreateAsync(applicationUserAirConditioning2, DefaultPassword);
        var createElectricityResult = await applicationUserService.CreateAsync(applicationUserElectricity, DefaultPassword);
        var createElectricity2Result = await applicationUserService.CreateAsync(applicationUserElectricity2, DefaultPassword);

        List<IdentityResult> createResults =
        [
            createClientResult, 
            createManagerResult, 
            createPlumbingResult, 
            createPlumbing2Result, 
            createAirConditioningResult,
            createAirConditioning2Result,
            createElectricityResult,
            createElectricity2Result
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



    private List<Location> CreateLocations(Location? parent, LocationType type, int first, int count)
    {
        return Enumerable.Range(first, count)
             .Select(x => Location.Create(LocationId.Create(), type, $"{type} {x}", parent).Value)
             .ToList();
    }

    private List<Location> GetLocations()
    {
        var buildings = CreateLocations(null, LocationType.Building, 1, 3)
            .ToList();

        var floors = buildings
            .SelectMany(building => CreateLocations(building, LocationType.Floor, int.Parse(building.Name), 4))
            .ToList();

        var departments = floors
            .SelectMany(floor => CreateLocations(floor, LocationType.Department, int.Parse(floor.Name), 4))
            .ToList();

        var rooms = departments
            .SelectMany(dep => CreateLocations(dep, LocationType.Room, int.Parse(dep.Name) * 2 + 100, 10))
            .ToList();


        return [.. buildings, .. floors, .. departments, .. rooms];
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
            Email = "plumbing1@medifix.com",
            UserName = "plumbing1@medifix.com",
            FirstName = "1",
            LastName = "Plumbing",
        };
    }

    private static ApplicationUser GetApplicationUserPlumbing2()
    {
        return new ApplicationUser
        {
            Type = UserType.Practitioner,
            Email = "plumbing2@medifix.com",
            UserName = "plumbing2@medifix.com",
            FirstName = "2",
            LastName = "Plumbing",
        };
    }

    private static ApplicationUser GetApplicationUserAirConditioning()
    {
        return new ApplicationUser
        {
            Type = UserType.Practitioner,
            Email = "air1@medifix.com",
            UserName = "air1@medifix.com",
            FirstName = "1",
            LastName = "Air conditioning",
        };
    }

    private static ApplicationUser GetApplicationUserAirConditioning2()
    {
        return new ApplicationUser
        {
            Type = UserType.Practitioner,
            Email = "air2@medifix.com",
            UserName = "air2@medifix.com",
            FirstName = "2",
            LastName = "Air conditioning",
        };
    }

    private static ApplicationUser GetApplicationUserElectricity()
    {
        return new ApplicationUser
        {
            Type = UserType.Practitioner,
            Email = "electricity1@medifix.com",
            UserName = "electricity1@medifix.com",
            FirstName = "1",
            LastName = "Electricity",
        };
    }

    private static ApplicationUser GetApplicationUserElectricity2()
    {
        return new ApplicationUser
        {
            Type = UserType.Practitioner,
            Email = "electricity2@medifix.com",
            UserName = "electricity2@medifix.com",
            FirstName = "2",
            LastName = "Electricity",
        };
    }
}
