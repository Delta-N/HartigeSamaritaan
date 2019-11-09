﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoosterPlanner.Data.Context;

namespace RoosterPlanner.Data.Migrations
{
    [DbContext(typeof(RoosterPlannerContext))]
    [Migration("20191109013551_Seed-Person")]
    partial class SeedPerson
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RoosterPlanner.Models.Category", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("11ae0153-5855-4147-8f62-46306ca248c7"),
                            Code = "KEUKEN",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3685),
                            Name = "Keuken"
                        },
                        new
                        {
                            Id = new Guid("b75c14ee-ffcc-4e23-92b2-165027b99c57"),
                            Code = "BEDIENING",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3688),
                            Name = "Bediening"
                        },
                        new
                        {
                            Id = new Guid("9a2e83ee-eee6-4022-8dd3-c6fb2966600d"),
                            Code = "LOGISTIEK",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3690),
                            Name = "Logistiek"
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Participation", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PersonId");

                    b.Property<Guid>("ProjectId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Participations");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Person", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<Guid>("Oid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            Id = new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3881),
                            Name = "Grace Hopper",
                            Oid = new Guid("b691f9f7-c404-4d52-a34f-c90702ca7138")
                        },
                        new
                        {
                            Id = new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3894),
                            Name = "John Wick",
                            Oid = new Guid("e2a94901-6942-4cfb-83fa-60343c0de219")
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Project", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Address")
                        .HasMaxLength(64);

                    b.Property<string>("City")
                        .HasMaxLength(64);

                    b.Property<bool>("Closed");

                    b.Property<string>("Description")
                        .HasMaxLength(512);

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("PictureUri");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("WebsiteUrl");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("RoosterPlanner.Models.ProjectTask", b =>
                {
                    b.Property<Guid>("ProjectId");

                    b.Property<Guid>("TaskId");

                    b.Property<Guid>("Id");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("ProjectId", "TaskId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("ProjectTasks");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Task", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<DateTime?>("DeletedDateTime");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("RoosterPlanner.Models.Participation", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Person", "Person")
                        .WithMany("Participations")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RoosterPlanner.Models.Project", "Project")
                        .WithMany("Participations")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RoosterPlanner.Models.ProjectTask", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Project", "Project")
                        .WithMany("ProjectTasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RoosterPlanner.Models.Task", "Task")
                        .WithMany("TaskProjects")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
