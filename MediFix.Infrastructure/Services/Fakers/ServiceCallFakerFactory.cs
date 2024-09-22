using Bogus;
using MediFix.Domain.Categories;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;

namespace MediFix.Infrastructure.Services.Fakers;

internal sealed class ServiceCallFakerFactory
{
    private readonly Faker _faker;
    private readonly Faker<ServiceCall> _serviceCallFaker;
    private readonly IEnumerable<Category> _categories;
    private readonly IEnumerable<SubCategory> _subCategories;
    private readonly IEnumerable<Practitioner> _practitioners;
    private readonly Guid _updateUserId;

    public ServiceCallFakerFactory(
        DateTime startDate,
        DateTime endDate,
        IEnumerable<Client> clients,
        IEnumerable<Location> rooms,
        IEnumerable<Category> categories,
        IEnumerable<SubCategory> subCategories,
        IEnumerable<Practitioner> practitioners,
        Guid updateUserId)
    {
        _categories = categories;
        _subCategories = subCategories;
        _practitioners = practitioners;
        _updateUserId = updateUserId;
        _faker = new Faker();

        _serviceCallFaker = new Faker<ServiceCall>()
            .CustomInstantiator(f => ServiceCall.Create(
                    f.PickRandom(clients).Id,
                    f.PickRandom(rooms).Id,
                    f.PickRandom<ServiceCallType>(),
                    f.PickRandom(_subCategories).Id,
                    f.Lorem.Text(),
                    f.PickRandom<ServiceCallPriority>())
                .Value)
            .RuleFor(sc => sc.DateCreated, f => f.Date.Between(startDate, endDate));
    }

    public List<ServiceCall> Generate(int count)
    {
        var serviceCalls = _serviceCallFaker.Generate(count);

        foreach (var serviceCall in serviceCalls)
        {
            GenerateStatusUpdates(serviceCall);
        }

        return serviceCalls;
    }

    private void GenerateStatusUpdates(ServiceCall serviceCall)
    {
        List<ServiceCallStatus> statuses = UpdateServiceCallStatusList();

        var practitionerId = GetRandomPractitionerId(serviceCall);

        UpdateServiceCallStatusesTimes(serviceCall, statuses, practitionerId);
    }

    private PractitionerId GetRandomPractitionerId(ServiceCall serviceCall)
    {
        var subCategory = _subCategories.Single(sub => serviceCall.SubCategoryId == sub.Id);

        var category = _categories
            .Single(c => c.Id == subCategory.CategoryId);

        var allowedPractitioner = _practitioners
            .Where(category.IsPractitionerAllowed);

        var practitionerId = _faker.PickRandom(allowedPractitioner).Id;

        return practitionerId;
    }

    private List<ServiceCallStatus> UpdateServiceCallStatusList()
    {
        // 10% chance of cancellation
        bool willBeCancelled = _faker.Random.Bool(0.1f);

        List<ServiceCallStatus> statuses = [ServiceCallStatus.New];

        if (!willBeCancelled && _faker.Random.Bool(0.8f)) // 80% chance to assign
        {
            statuses.Add(ServiceCallStatus.AssignedToPractitioner);

            if (_faker.Random.Bool(0.85f)) // 85% chance to start
            {
                statuses.Add(ServiceCallStatus.Started);

                if (_faker.Random.Bool(0.8f)) // 80% chance to finish if started
                {
                    statuses.Add(ServiceCallStatus.Finished);
                }
            }
        }
        else if (willBeCancelled)
        {
            if (_faker.Random.Bool(0.3f)) // 30% chance to be assigned before cancellation
            {
                statuses.Add(ServiceCallStatus.AssignedToPractitioner);
            }
            statuses.Add(ServiceCallStatus.Cancelled);
        }

        return statuses;
    }

    private void UpdateServiceCallStatusesTimes(
        ServiceCall serviceCall,
        List<ServiceCallStatus> statuses,
        PractitionerId practitionerId)
    {
        foreach (var status in statuses)
        {
            var dateFrom = serviceCall.CurrentStatus.DateTime.AddMinutes(1);
            DateTime currentDate;

            switch (status)
            {
                case ServiceCallStatus.New:
                    currentDate = serviceCall.DateCreated;
                    break;
                case ServiceCallStatus.AssignedToPractitioner:
                    serviceCall.AssignPractitioner(_updateUserId, practitionerId);
                    currentDate = _faker.Date.Between(dateFrom, dateFrom.AddHours(_faker.Random.Number(0, 3)));
                    break;
                case ServiceCallStatus.Started:
                    serviceCall.Start(practitionerId);
                    currentDate = _faker.Date.Between(dateFrom, dateFrom.AddMinutes(_faker.Random.Number(10, 60 * 5)));
                    break;
                case ServiceCallStatus.Finished:
                    serviceCall.Finish(practitionerId, _faker.Lorem.Sentence());
                    currentDate = _faker.Date.Between(dateFrom, dateFrom.AddMinutes(_faker.Random.Number(5, 120)));
                    break;
                case ServiceCallStatus.Cancelled:
                    serviceCall.Cancel(_updateUserId);
                    currentDate = _faker.Date.Between(dateFrom, dateFrom.AddHours(_faker.Random.Number(0, 2)));
                    break;
                default:
                    throw new InvalidOperationException();
            }

            // Hack to set the correct date for each status update
            var lastUpdate = serviceCall.StatusHistory.Last();
            typeof(ServiceCallStatusUpdate).GetProperty(nameof(ServiceCallStatusUpdate.DateTime))!
                .SetValue(lastUpdate, currentDate);
        }
    }
}