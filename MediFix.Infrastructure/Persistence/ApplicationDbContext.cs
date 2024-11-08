﻿using MediFix.Application.Users.Entities;
using MediFix.Domain.Categories;
using MediFix.Domain.Expertises;
using MediFix.Domain.Locations;
using MediFix.Domain.ServiceCalls;
using MediFix.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MediFix.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<SubCategory> SubCategories { get; set; } = null!;
    public DbSet<ServiceCall> ServiceCalls { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Manager> Managers { get; set; } = null!;
    public DbSet<Practitioner> Practitioners { get; set; } = null!;
    public DbSet<Expertise> Expertises { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        ConfigurePrimaryKeys(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigurePrimaryKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.IsPrimaryKey())
            .ToList()
            .ForEach(p => p.ValueGenerated = ValueGenerated.Never);
    }
}