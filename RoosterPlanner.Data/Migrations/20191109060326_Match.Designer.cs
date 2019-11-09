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
    [Migration("20191109060326_Match")]
    partial class Match
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
                            Id = new Guid("5b818cab-5dfc-44c9-871b-d0fe896ae2d2"),
                            Code = "KEUKEN",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 11, 9, 6, 3, 26, 569, DateTimeKind.Utc).AddTicks(8018),
                            Name = "Keuken"
                        },
                        new
                        {
                            Id = new Guid("5a35aee6-3813-4ac1-b532-f50f9d8f5237"),
                            Code = "BEDIENING",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 11, 9, 6, 3, 26, 569, DateTimeKind.Utc).AddTicks(8027),
                            Name = "Bediening"
                        },
                        new
                        {
                            Id = new Guid("d16e5fc3-5c96-4f25-a306-9c4978ecbb15"),
                            Code = "LOGISTIEK",
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 11, 9, 6, 3, 26, 569, DateTimeKind.Utc).AddTicks(8031),
                            Name = "Logistiek"
                        });
                });

            modelBuilder.Entity("RoosterPlanner.Models.Match", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("LastEditBy")
                        .HasMaxLength(128);

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ParticipationId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<Guid>("TaskId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ParticipationId");

                    b.HasIndex("TaskId");

                    b.ToTable("Matches");
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
                            LastEditDate = new DateTime(2019, 11, 9, 6, 3, 26, 569, DateTimeKind.Utc).AddTicks(8424),
                            Name = "Grace Hopper",
                            Oid = new Guid("b691f9f7-c404-4d52-a34f-c90702ca7138")
                        },
                        new
                        {
                            Id = new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                            LastEditBy = "System",
                            LastEditDate = new DateTime(2019, 11, 9, 6, 3, 26, 569, DateTimeKind.Utc).AddTicks(8446),
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

            modelBuilder.Entity("RoosterPlanner.Models.Match", b =>
                {
                    b.HasOne("RoosterPlanner.Models.Participation", "Participation")
                        .WithMany()
                        .HasForeignKey("ParticipationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RoosterPlanner.Models.Participation", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
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
