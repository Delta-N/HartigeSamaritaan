﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoosterPlanner.Data.Context;

namespace RoosterPlanner.Data.Migrations
{
    [DbContext(typeof(RoosterPlannerContext))]
    partial class RoosterPlannerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RoosterPlanner.Models.Availability", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ParticipationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Preference")
                        .HasColumnType("bit");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("ShiftId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParticipationId");

                    b.HasIndex("ShiftId");

                    b.ToTable("Availabilities");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("UrlPdf")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"),
                            Code = "KEUKEN",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 1, 22, 8, 1, 1, 0, DateTimeKind.Unspecified),
                            Name = "Keuken"
                        },
                        new
                        {
                            Id = new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"),
                            Code = "BEDIENING",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 1, 18, 16, 55, 29, 0, DateTimeKind.Unspecified),
                            Name = "Bediening"
                        },
                        new
                        {
                            Id = new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"),
                            Code = "LOGISTIEK",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified),
                            Name = "Logistiek"
                        },
                        new
                        {
                            Id = new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"),
                            Code = "OVERIGE",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified),
                            Name = "Overige"
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Certificate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CertificateTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateExpired")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateIssued")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("CertificateTypeId");

                    b.HasIndex("PersonId");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("RoosterPlanner.Models.CertificateType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("CertificateTypes");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Collaboration", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("IsWantedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid?>("WantsToWorkWithId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("IsWantedById");

                    b.HasIndex("WantsToWorkWithId");

                    b.ToTable("Collaborations");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Participation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxWorkingHoursPerWeek")
                        .HasColumnType("int");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Participations");

                    b.HasData(
                        new
                        {
                            Id = new Guid("66e971cf-16f2-4521-befb-aaca981f642f"),
                            LastEditDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxWorkingHoursPerWeek = 12,
                            PersonId = new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                            ProjectId = new Guid("e86bb765-27ab-404f-b140-211505d869fe")
                        },
                        new
                        {
                            Id = new Guid("541310c7-ffec-43f5-81a7-7b2c07f9ce81"),
                            LastEditDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            MaxWorkingHoursPerWeek = 40,
                            PersonId = new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                            ProjectId = new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a")
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Oid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Oid")
                        .IsUnique();

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            Id = new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 1, 1, 12, 34, 28, 0, DateTimeKind.Unspecified),
                            Oid = new Guid("b691f9f7-c404-4d52-a34f-c90702ca7138"),
                            Type = 0,
                            firstName = "Grace"
                        },
                        new
                        {
                            Id = new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 1, 2, 12, 45, 1, 0, DateTimeKind.Unspecified),
                            Oid = new Guid("e2a94901-6942-4cfb-83fa-60343c0de219"),
                            Type = 0,
                            firstName = "John"
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<bool>("Closed")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ParticipationEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ParticipationStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PictureUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProjectEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ProjectStartDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e86bb765-27ab-404f-b140-211505d869fe"),
                            Address = "Stationsplein 2",
                            City = "Voorburg",
                            Closed = false,
                            Description = "Leuk project in Voorburg",
                            LastEditDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Voorburg 2020",
                            ParticipationEndDate = new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ParticipationStartDate = new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProjectEndDate = new DateTime(2020, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProjectStartDate = new DateTime(2020, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"),
                            Address = "Laan van Waalhaven 450",
                            City = "Den Haag",
                            Closed = false,
                            Description = "Leuk project in Den Haag",
                            LastEditDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Den Haag 2018",
                            ParticipationEndDate = new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ParticipationStartDate = new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProjectEndDate = new DateTime(2020, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ProjectStartDate = new DateTime(2020, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.ProjectTask", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid?>("TaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TaskId");

                    b.ToTable("ProjectTasks");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Requirement", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CertificateTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid?>("TaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CertificateTypeId");

                    b.HasIndex("TaskId");

                    b.ToTable("Requirements");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Shift", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<Guid?>("TaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TaskId");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("DocumentUri")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("LastEditBy")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Tasks");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ce45a6a5-a41a-4fa5-887f-d67b13fbad94"),
                            CategoryId = new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"),
                            Color = "Blue",
                            Description = "Een leuke beschrijving van de werkzaamheden van een chef",
                            DocumentUri = "http://test.com/chef",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chef"
                        },
                        new
                        {
                            Id = new Guid("202b5144-8232-4531-a080-60cca12919d1"),
                            CategoryId = new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"),
                            Color = "Red",
                            Description = "Een leuke beschrijving van de werkzaamheden van een runner",
                            DocumentUri = "http://test.com/runner",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Runner"
                        },
                        new
                        {
                            Id = new Guid("0ca10a56-cf22-4afe-a56b-a56a4c496631"),
                            CategoryId = new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"),
                            Color = "Yellow",
                            Description = "Een leuke beschrijving van de werkzaamheden van een chauffeur",
                            DocumentUri = "http://test.com/chauffeur",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Chauffeur"
                        },
                        new
                        {
                            Id = new Guid("b4394291-861d-4ed8-b13e-9b4d5f105dc5"),
                            CategoryId = new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"),
                            Color = "Green",
                            Description = "Een leuke beschrijving van de werkzaamheden van een klusser",
                            DocumentUri = "http://test.com/Klusser",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Klusser"
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Availability", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Participation", "Participation")
                        .WithMany("Availabilities")
                        .HasForeignKey("ParticipationId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RoosterPlanner.Models.Shift", "Shift")
                        .WithMany("Availabilities")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("RoosterPlanner.Models.Certificate", b =>
                {
                    b.HasOne("RoosterPlanner.Models.CertificateType", "CertificateType")
                        .WithMany("Certificates")
                        .HasForeignKey("CertificateTypeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RoosterPlanner.Models.Person", "Person")
                        .WithMany("Certificates")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoosterPlanner.Models.Collaboration", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Participation", "IsWantedBy")
                        .WithMany("IsWantedBy")
                        .HasForeignKey("IsWantedById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RoosterPlanner.Models.Participation", "WantsToWorkWith")
                        .WithMany("WantsToWorkWith")
                        .HasForeignKey("WantsToWorkWithId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("RoosterPlanner.Models.Participation", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Person", "Person")
                        .WithMany("Participations")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoosterPlanner.Models.Project", "Project")
                        .WithMany("Participations")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoosterPlanner.Models.ProjectTask", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Project", "Project")
                        .WithMany("ProjectTasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoosterPlanner.Models.Task", "Task")
                        .WithMany("ProjectTasks")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("RoosterPlanner.Models.Requirement", b =>
                {
                    b.HasOne("RoosterPlanner.Models.CertificateType", "CertificateType")
                        .WithMany("Requirements")
                        .HasForeignKey("CertificateTypeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RoosterPlanner.Models.Task", "Task")
                        .WithMany("Requirements")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("RoosterPlanner.Models.Shift", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Project", "Project")
                        .WithMany("Shifts")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoosterPlanner.Models.Task", "Task")
                        .WithMany("Shifts")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("RoosterPlanner.Models.Task", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Category", "Category")
                        .WithMany("Tasks")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
