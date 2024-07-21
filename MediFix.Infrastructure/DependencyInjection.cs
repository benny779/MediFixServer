﻿using Dapper;
using MediFix.Application.Abstractions.Data;
using MediFix.Application.Abstractions.Services;
using MediFix.Application.Users;
using MediFix.Application.Users.Entities;
using MediFix.Application.Utils.Persistence;
using MediFix.Infrastructure.Authentication;
using MediFix.Infrastructure.Persistence;
using MediFix.Infrastructure.Persistence.Abstractions;
using MediFix.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
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

        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddScoped<IPersistenceService, PersistenceService>();

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager<SignInManager<ApplicationUser>>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MediFix")));

        AddRepositories(services);

        AddStronglyTypedIdMappers();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        return services;
    }

    private static void AddStronglyTypedIdMappers()
    {
        var assembly = typeof(DependencyInjection).Assembly;

        var typeHandlerType = typeof(SqlMapper.TypeHandler<>);

        var handlerTypes = assembly.GetTypes()
            .Where(t => t.BaseType is { IsGenericType: true } 
                        && t.BaseType.GetGenericTypeDefinition() == typeHandlerType);

        foreach (var handlerType in handlerTypes)
        {
            var handlerInstance = Activator.CreateInstance(handlerType);
            var handledType = handlerType.BaseType.GetGenericArguments()[0];
            SqlMapper.AddTypeHandler(handledType, (SqlMapper.ITypeHandler)handlerInstance);
        }
    }

    private static void AddRepositories(IServiceCollection services)
    {
        var applicationAssembly = typeof(Application.DependencyInjection).Assembly;

        var assembly = typeof(DependencyInjection).Assembly;

        var repositoryInterfaces = applicationAssembly.GetTypes()
            .Where(i => i.IsInterface && i.Name.StartsWith('I') && i.Name.EndsWith("Repository"))
            .ToList();

        var repositoryTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.Name.EndsWith("Repository"))
            .ToList();

        var repositories = repositoryInterfaces
            .Select(repoInterface => new KeyValuePair<Type, Type?>(
                repoInterface,
                repositoryTypes
                    .FirstOrDefault(repoType => repoType.Name.Equals(repoInterface.Name[1..])
                        && repoType.GetInterfaces().Contains(repoInterface))))
            .Where(pair => pair.Value is not null)
            .ToList();

        foreach (var repo in repositories)
        {
            services.AddScoped(repo.Key, repo.Value!);
        }
    }
}