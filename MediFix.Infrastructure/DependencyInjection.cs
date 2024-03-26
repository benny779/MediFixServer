using MediFix.Application.Users;
using MediFix.Application.Users.CreateUser;
using MediFix.Infrastructure.Persistence;
using MediFix.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediFix.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MediFix")));
            //options.UseSqlServer());

        services.AddScoped<IUsersRepository, UserRepository>();
        //services.AddScoped<IServiceCallRepository, ServiceCallRepository>();

        return services;
    }
}