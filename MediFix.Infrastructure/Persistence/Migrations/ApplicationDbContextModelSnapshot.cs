﻿// <auto-generated />
using System;
using MediFix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MediFix.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExpertisePractitioner", b =>
                {
                    b.Property<Guid>("ExpertisesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PractitionersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ExpertisesId", "PractitionersId");

                    b.HasIndex("PractitionersId");

                    b.ToTable("PractitionerExpertise", (string)null);
                });

            modelBuilder.Entity("MediFix.Application.Users.Entities.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("RefreshTokenValidity")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("MediFix.Domain.Categories.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ba945cd3-3ed5-45f1-9879-7385f559930a"),
                            Name = "Plumbing"
                        },
                        new
                        {
                            Id = new Guid("d89357de-3e45-428c-8ab0-36eb7c043ab0"),
                            Name = "Air conditioning"
                        });
                });

            modelBuilder.Entity("MediFix.Domain.Categories.SubCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Name", "CategoryId")
                        .IsUnique();

                    b.ToTable("SubCategories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5a0dc366-fe58-4f54-9be3-ceeff5373d8a"),
                            CategoryId = new Guid("ba945cd3-3ed5-45f1-9879-7385f559930a"),
                            Name = "Toilet"
                        },
                        new
                        {
                            Id = new Guid("d3eb3af9-6378-4d77-9217-3493c89021c4"),
                            CategoryId = new Guid("ba945cd3-3ed5-45f1-9879-7385f559930a"),
                            Name = "Tap"
                        },
                        new
                        {
                            Id = new Guid("2510bbb0-0ebd-415a-94e3-e68763d6954b"),
                            CategoryId = new Guid("ba945cd3-3ed5-45f1-9879-7385f559930a"),
                            Name = "Water Bar"
                        },
                        new
                        {
                            Id = new Guid("c7b3d80e-ee2e-4523-86c7-cd0a9bda7598"),
                            CategoryId = new Guid("d89357de-3e45-428c-8ab0-36eb7c043ab0"),
                            Name = "Air conditioner does not cool"
                        },
                        new
                        {
                            Id = new Guid("7bb1cad4-cb5c-47d0-9653-1a297ba52ef2"),
                            CategoryId = new Guid("d89357de-3e45-428c-8ab0-36eb7c043ab0"),
                            Name = "Noisy air conditioner"
                        });
                });

            modelBuilder.Entity("MediFix.Domain.Locations.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<byte>("LocationType")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("Name", "LocationType", "ParentId")
                        .IsUnique();

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            Id = new Guid("964bac8b-1471-4633-a648-116d4732898d"),
                            IsActive = true,
                            LocationType = (byte)1,
                            Name = "A"
                        },
                        new
                        {
                            Id = new Guid("039d3ab6-9fc8-4b25-9f4b-595722859132"),
                            IsActive = true,
                            LocationType = (byte)2,
                            Name = "0",
                            ParentId = new Guid("964bac8b-1471-4633-a648-116d4732898d")
                        },
                        new
                        {
                            Id = new Guid("031c96ed-4525-46c8-8f9f-2907dc78c3d5"),
                            IsActive = true,
                            LocationType = (byte)3,
                            Name = "HR",
                            ParentId = new Guid("039d3ab6-9fc8-4b25-9f4b-595722859132")
                        },
                        new
                        {
                            Id = new Guid("2ddbb051-08d7-4cba-9815-6b967ea5007f"),
                            IsActive = true,
                            LocationType = (byte)3,
                            Name = "IT",
                            ParentId = new Guid("039d3ab6-9fc8-4b25-9f4b-595722859132")
                        },
                        new
                        {
                            Id = new Guid("342d34d9-a72d-4ae1-be89-db3fdce6bf3d"),
                            IsActive = true,
                            LocationType = (byte)4,
                            Name = "100",
                            ParentId = new Guid("031c96ed-4525-46c8-8f9f-2907dc78c3d5")
                        },
                        new
                        {
                            Id = new Guid("0a984d3d-986c-4fd2-a031-67025661117e"),
                            IsActive = true,
                            LocationType = (byte)4,
                            Name = "101",
                            ParentId = new Guid("031c96ed-4525-46c8-8f9f-2907dc78c3d5")
                        },
                        new
                        {
                            Id = new Guid("43c193b7-24a9-4cfd-9ae0-ea89ff52389c"),
                            IsActive = true,
                            LocationType = (byte)4,
                            Name = "102",
                            ParentId = new Guid("031c96ed-4525-46c8-8f9f-2907dc78c3d5")
                        },
                        new
                        {
                            Id = new Guid("fee896ae-5c68-4c99-b761-070f8e36863a"),
                            IsActive = true,
                            LocationType = (byte)4,
                            Name = "200",
                            ParentId = new Guid("2ddbb051-08d7-4cba-9815-6b967ea5007f")
                        },
                        new
                        {
                            Id = new Guid("c173b638-df64-4159-a6a7-1148c3e7ea7f"),
                            IsActive = true,
                            LocationType = (byte)4,
                            Name = "201",
                            ParentId = new Guid("2ddbb051-08d7-4cba-9815-6b967ea5007f")
                        },
                        new
                        {
                            Id = new Guid("5f49dc80-72cf-4217-b778-9e3d2c734436"),
                            IsActive = true,
                            LocationType = (byte)4,
                            Name = "202",
                            ParentId = new Guid("2ddbb051-08d7-4cba-9815-6b967ea5007f")
                        });
                });

            modelBuilder.Entity("MediFix.Domain.ServiceCalls.ServiceCall", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Priority")
                        .HasColumnType("tinyint");

                    b.Property<byte>("ServiceCallType")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("SubCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("LocationId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("ServiceCalls");
                });

            modelBuilder.Entity("MediFix.Domain.Users.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("MediFix.Domain.Users.Expertise", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Expertises");
                });

            modelBuilder.Entity("MediFix.Domain.Users.Manager", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("MediFix.Domain.Users.Practitioner", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Practitioners");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ExpertisePractitioner", b =>
                {
                    b.HasOne("MediFix.Domain.Users.Expertise", null)
                        .WithMany()
                        .HasForeignKey("ExpertisesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediFix.Domain.Users.Practitioner", null)
                        .WithMany()
                        .HasForeignKey("PractitionersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MediFix.Domain.Categories.SubCategory", b =>
                {
                    b.HasOne("MediFix.Domain.Categories.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MediFix.Domain.Locations.Location", b =>
                {
                    b.HasOne("MediFix.Domain.Locations.Location", null)
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("MediFix.Domain.ServiceCalls.ServiceCall", b =>
                {
                    b.HasOne("MediFix.Domain.Users.Client", null)
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediFix.Domain.Locations.Location", null)
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediFix.Domain.Categories.SubCategory", null)
                        .WithMany()
                        .HasForeignKey("SubCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("MediFix.Domain.ServiceCalls.ServiceCallStatusUpdate", "StatusHistory", b1 =>
                        {
                            b1.Property<Guid>("ServiceCallId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("DateTime")
                                .HasColumnType("datetime2");

                            b1.Property<Guid?>("PractitionerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<byte>("Status")
                                .HasColumnType("tinyint");

                            b1.Property<Guid>("UpdatedBy")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("ServiceCallId", "DateTime");

                            b1.HasIndex("PractitionerId");

                            b1.ToTable("ServiceCallStatusUpdate");

                            b1.HasOne("MediFix.Domain.Users.Practitioner", null)
                                .WithMany()
                                .HasForeignKey("PractitionerId")
                                .OnDelete(DeleteBehavior.NoAction);

                            b1.WithOwner()
                                .HasForeignKey("ServiceCallId");
                        });

                    b.Navigation("StatusHistory");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("MediFix.Application.Users.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("MediFix.Application.Users.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediFix.Application.Users.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("MediFix.Application.Users.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}