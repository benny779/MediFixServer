using MediFix.Application.ServiceCalls;
using MediFix.Domain.ServiceCalls;
using MediFix.Infrastructure.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MediFix.Infrastructure.Persistence.Repositories;

public class ServiceCallRepository(ApplicationDbContext dbContext)
    : Repository<ServiceCall, ServiceCallId>(dbContext)
        , IServiceCallRepository
{
    public IQueryable<ServiceCallResponse> ToResponse(Expression<Func<ServiceCall, bool>> predicate)
    {
        return ToResponse(dbContext.ServiceCalls.Where(predicate));
    }

    public IQueryable<ServiceCallResponse> ToResponse(IQueryable<ServiceCall> serviceCalls)
    {
        return from sc in serviceCalls
               join client in dbContext.Clients on sc.ClientId equals client.Id
               join clientAppUser in dbContext.Users on client.Id equals clientAppUser.Id
               join room in dbContext.Locations on sc.LocationId equals room.Id
               join dep in dbContext.Locations on room.ParentId equals dep.Id
               join floor in dbContext.Locations on dep.ParentId equals floor.Id
               join building in dbContext.Locations on floor.ParentId equals building.Id
               join subcat in dbContext.SubCategories on sc.SubCategoryId equals subcat.Id
               join cat in dbContext.Categories on subcat.CategoryId equals cat.Id
               join practitioner in dbContext.Practitioners on sc.PractitionerId equals practitioner.Id into practitionerGroup
               from practitioner in practitionerGroup.DefaultIfEmpty()
               join pracAppUser in dbContext.Users on practitioner.Id equals pracAppUser.Id into pracUserGroup
               from pracAppUser in pracUserGroup.DefaultIfEmpty()
               select new ServiceCallResponse(
                    sc.Id,
                    new ServiceCallClient(
                        client.Id,
                        clientAppUser.FullName,
                        clientAppUser.Email,
                        clientAppUser.PhoneNumber),
                    new ServiceCallLocations(
                        new ServiceCallLocation(
                            building.Id,
                            building.LocationType,
                            building.Name),
                        new ServiceCallLocation(
                            floor.Id,
                            floor.LocationType,
                            floor.Name),
                        new ServiceCallLocation(
                            dep.Id,
                            dep.LocationType,
                            dep.Name),
                        new ServiceCallLocation(
                            room.Id,
                            room.LocationType,
                            room.Name)),
                    sc.ServiceCallType,
                    new ServiceCallCategory(
                        cat.Id,
                        cat.Name),
                    new ServiceCallSubCategory(
                        subcat.Id,
                        subcat.Name),
                    sc.Details,
                    sc.DateCreated,
                    sc.Priority,
                    sc.StatusHistory.ToList(),
                    sc.CurrentStatus,
                    sc.CloseDetails,
                    pracAppUser != null
                        ? new ServiceCallPractitioner(
                            pracAppUser.Id,
                            pracAppUser.FullName,
                            pracAppUser.Email,
                            pracAppUser.PhoneNumber)
                        : null);
    }

    public override IQueryable<ServiceCall> GetQueryableWithNavigation()
    {
        return dbContext
            .ServiceCalls
            .Include(sc => sc.StatusHistory);
    }
}
