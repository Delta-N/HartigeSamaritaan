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
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("RoosterPlanner.Models.Availability", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

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
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"),
                            Code = "KEUKEN",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 259, DateTimeKind.Local).AddTicks(2311),
                            Name = "Keuken"
                        },
                        new
                        {
                            Id = new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"),
                            Code = "BEDIENING",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 259, DateTimeKind.Local).AddTicks(2630),
                            Name = "Bediening"
                        },
                        new
                        {
                            Id = new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"),
                            Code = "LOGISTIEK",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 259, DateTimeKind.Local).AddTicks(2644),
                            Name = "Logistiek"
                        },
                        new
                        {
                            Id = new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"),
                            Code = "OVERIGE",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 259, DateTimeKind.Local).AddTicks(2648),
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
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

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
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("CertificateTypes");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DocumentUri")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Documents");

                    b.HasData(
                        new
                        {
                            Id = new Guid("103d6c9a-91d4-4d76-bff5-c3f829fcd1a9"),
                            DocumentUri = "https://hackatonstoragedev.blob.core.windows.net/projectpicture/037efc9a-0836-4513-8ae3-282c597631c4.jfif",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 262, DateTimeKind.Local).AddTicks(4938),
                            Name = "TermsOfService"
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Manager", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

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

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Participation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

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
                            Active = true,
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(5753),
                            MaxWorkingHoursPerWeek = 12,
                            PersonId = new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                            ProjectId = new Guid("e86bb765-27ab-404f-b140-211505d869fe")
                        },
                        new
                        {
                            Id = new Guid("541310c7-ffec-43f5-81a7-7b2c07f9ce81"),
                            Active = true,
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(5786),
                            MaxWorkingHoursPerWeek = 40,
                            PersonId = new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                            ProjectId = new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a")
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Oid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PersonalRemark")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid?>("ProfilePictureId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("StaffRemark")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Oid")
                        .IsUnique();

                    b.HasIndex("ProfilePictureId");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            Id = new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                            FirstName = "Grace",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 1, 1, 12, 34, 28, 0, DateTimeKind.Unspecified),
                            Oid = new Guid("b691f9f7-c404-4d52-a34f-c90702ca7138")
                        },
                        new
                        {
                            Id = new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                            FirstName = "John",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 1, 2, 12, 45, 1, 0, DateTimeKind.Unspecified),
                            Oid = new Guid("e2a94901-6942-4cfb-83fa-60343c0de219")
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("City")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("Closed")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime?>("ParticipationEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ParticipationStartDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("PictureUriId")
                        .HasColumnType("uniqueidentifier");

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

                    b.HasIndex("PictureUriId");

                    b.ToTable("Projects");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e86bb765-27ab-404f-b140-211505d869fe"),
                            Address = "Stationsplein 2",
                            City = "Voorburg",
                            Closed = false,
                            Description = "Leuk project in Voorburg",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(1696),
                            Name = "Voorburg 2020",
                            ParticipationEndDate = new DateTime(2021, 1, 29, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(1061),
                            ParticipationStartDate = new DateTime(2021, 1, 1, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(801),
                            ProjectEndDate = new DateTime(2021, 1, 30, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(1514),
                            ProjectStartDate = new DateTime(2020, 12, 31, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(1303)
                        },
                        new
                        {
                            Id = new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"),
                            Address = "Laan van Waalhaven 450",
                            City = "Den Haag",
                            Closed = false,
                            Description = "Leuk project in Den Haag",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(1736),
                            Name = "Den Haag 2018",
                            ParticipationEndDate = new DateTime(2021, 1, 29, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(1729),
                            ParticipationStartDate = new DateTime(2021, 1, 1, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(1726),
                            ProjectEndDate = new DateTime(2021, 1, 30, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(1734),
                            ProjectStartDate = new DateTime(2020, 12, 31, 8, 56, 19, 261, DateTimeKind.Local).AddTicks(1732)
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.ProjectTask", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TaskId");

                    b.ToTable("ProjectTasks");
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
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

                    b.Property<int>("ParticipantsRequired")
                        .HasColumnType("int");

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
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid?>("InstructionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("InstructionId");

                    b.ToTable("Tasks");

                    b.HasData(
                        new
                        {
                            Id = new Guid("09d1bef0-c6fd-4b28-a17f-7f368acb8129"),
                            CategoryId = new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"),
                            Color = "Blue",
                            Description = "Een leuke beschrijving van de werkzaamheden van een chef",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 262, DateTimeKind.Local).AddTicks(1593),
                            Name = "Chef"
                        },
                        new
                        {
                            Id = new Guid("b1f63215-2b1c-4a74-bc39-e8fc17dbf4e0"),
                            CategoryId = new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"),
                            Color = "Red",
                            Description = "Een leuke beschrijving van de werkzaamheden van een runner",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 262, DateTimeKind.Local).AddTicks(1632),
                            Name = "Runner"
                        },
                        new
                        {
                            Id = new Guid("f5939453-f9cb-4402-96ea-6e7fc81e3b62"),
                            CategoryId = new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"),
                            Color = "Yellow",
                            Description = "Een leuke beschrijving van de werkzaamheden van een chauffeur",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 262, DateTimeKind.Local).AddTicks(1640),
                            Name = "Chauffeur"
                        },
                        new
                        {
                            Id = new Guid("e1a6b6c8-7d91-477f-ad2e-a42bbd63fd61"),
                            CategoryId = new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"),
                            Color = "Green",
                            Description = "Een leuke beschrijving van de werkzaamheden van een klusser",
                            LastEditBy = "SYSTEM",
                            LastEditDate = new DateTime(2020, 12, 31, 8, 56, 19, 262, DateTimeKind.Local).AddTicks(1645),
                            Name = "Klusser"
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Availability", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Participation", "Participation")
                        .WithMany("Availabilities")
                        .HasForeignKey("ParticipationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("RoosterPlanner.Models.Shift", "Shift")
                        .WithMany("Availabilities")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Participation");

                    b.Navigation("Shift");
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

                    b.Navigation("CertificateType");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Manager", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoosterPlanner.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Project");
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

                    b.Navigation("Person");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Person", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Document", "ProfilePicture")
                        .WithMany("ProfilePictures")
                        .HasForeignKey("ProfilePictureId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Project", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Document", "PictureUri")
                        .WithMany("ProjectPictures")
                        .HasForeignKey("PictureUriId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("PictureUri");
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
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("Task");
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

                    b.Navigation("Project");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Task", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Category", "Category")
                        .WithMany("Tasks")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RoosterPlanner.Models.Document", "Instruction")
                        .WithMany("Instructions")
                        .HasForeignKey("InstructionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Category");

                    b.Navigation("Instruction");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Category", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("RoosterPlanner.Models.CertificateType", b =>
                {
                    b.Navigation("Certificates");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Document", b =>
                {
                    b.Navigation("Instructions");

                    b.Navigation("ProfilePictures");

                    b.Navigation("ProjectPictures");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Participation", b =>
                {
                    b.Navigation("Availabilities");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Person", b =>
                {
                    b.Navigation("Certificates");

                    b.Navigation("Participations");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Project", b =>
                {
                    b.Navigation("Participations");

                    b.Navigation("ProjectTasks");

                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Shift", b =>
                {
                    b.Navigation("Availabilities");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Task", b =>
                {
                    b.Navigation("ProjectTasks");

                    b.Navigation("Shifts");
                });
#pragma warning restore 612, 618
        }
    }
}
