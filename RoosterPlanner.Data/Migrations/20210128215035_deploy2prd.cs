using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class deploy2prd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Level = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DocumentUri = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePictureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PersonalRemark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    StaffRemark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PushDisabled = table.Column<bool>(type: "bit", nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Documents_ProfilePictureId",
                        column: x => x.ProfilePictureId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    City = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ParticipationStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParticipationEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PictureUriId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    ContactAdres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Documents_PictureUriId",
                        column: x => x.PictureUriId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    InstructionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tasks_Documents_InstructionId",
                        column: x => x.InstructionId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateExpired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertificateTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_CertificateTypes_CertificateTypeId",
                        column: x => x.CertificateTypeId,
                        principalTable: "CertificateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Certificates_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Managers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Managers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaxWorkingHoursPerWeek = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participations_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertificateTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requirements_CertificateTypes_CertificateTypeId",
                        column: x => x.CertificateTypeId,
                        principalTable: "CertificateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requirements_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantsRequired = table.Column<int>(type: "int", nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shifts_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Preference = table.Column<bool>(type: "bit", nullable: false),
                    PushEmailSend = table.Column<bool>(type: "bit", nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Availabilities_Participations_ParticipationId",
                        column: x => x.ParticipationId,
                        principalTable: "Participations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Availabilities_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[,]
                {
                    { new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "KEUKEN", "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 992, DateTimeKind.Local).AddTicks(3856), "Keuken" },
                    { new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "BEDIENING", "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 992, DateTimeKind.Local).AddTicks(4145), "Bediening" },
                    { new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "LOGISTIEK", "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 992, DateTimeKind.Local).AddTicks(4159), "Logistiek" },
                    { new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "OVERIGE", "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 992, DateTimeKind.Local).AddTicks(4163), "Overige" }
                });

            migrationBuilder.InsertData(
                table: "CertificateTypes",
                columns: new[] { "Id", "LastEditBy", "LastEditDate", "Level", "Name" },
                values: new object[,]
                {
                    { new Guid("249058f0-d5e5-4684-82c5-6fd6461b36f4"), "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 994, DateTimeKind.Local).AddTicks(7137), null, "Biefstuk-capable" },
                    { new Guid("938b16bd-0a1d-4c1c-bff1-4a32e0e32ca7"), "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 994, DateTimeKind.Local).AddTicks(7351), "B", "Rijbewijs" },
                    { new Guid("dbadad9c-7a6f-479f-a76a-00f51e5c8e98"), "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 994, DateTimeKind.Local).AddTicks(7374), null, "Barcertificaat" },
                    { new Guid("907941fd-7bff-4f6d-9054-c02bc1bc8e3a"), "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 994, DateTimeKind.Local).AddTicks(7390), null, "HACCP" }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CategoryId", "Color", "Description", "InstructionId", "LastEditBy", "LastEditDate", "Name" },
                values: new object[,]
                {
                    { new Guid("2ef66051-baf8-4158-92a5-2c63b581a48d"), new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "Blue", "Een leuke beschrijving van de werkzaamheden van een chef", null, "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 994, DateTimeKind.Local).AddTicks(1811), "Chef" },
                    { new Guid("614334c4-7139-4801-b9f8-9e6f1baa024b"), new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "Red", "Een leuke beschrijving van de werkzaamheden van een runner", null, "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 994, DateTimeKind.Local).AddTicks(1856), "Runner" },
                    { new Guid("2594c71a-55b5-435d-b629-9d6eafe3f0e6"), new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "Yellow", "Een leuke beschrijving van de werkzaamheden van een chauffeur", null, "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 994, DateTimeKind.Local).AddTicks(1865), "Chauffeur" },
                    { new Guid("5ae889a0-6b64-4806-9bd2-9cc9bee3a247"), new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "Green", "Een leuke beschrijving van de werkzaamheden van een klusser", null, "SYSTEM", new DateTime(2021, 1, 28, 22, 50, 33, 994, DateTimeKind.Local).AddTicks(1870), "Klusser" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_ParticipationId",
                table: "Availabilities",
                column: "ParticipationId");

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_ShiftId",
                table: "Availabilities",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_CertificateTypeId",
                table: "Certificates",
                column: "CertificateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_PersonId",
                table: "Certificates",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_PersonId",
                table: "Managers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_ProjectId",
                table: "Managers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_PersonId",
                table: "Participations",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Participations_ProjectId",
                table: "Participations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Oid",
                table: "Persons",
                column: "Oid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ProfilePictureId",
                table: "Persons",
                column: "ProfilePictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PictureUriId",
                table: "Projects",
                column: "PictureUriId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_TaskId",
                table: "ProjectTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_CertificateTypeId_TaskId",
                table: "Requirements",
                columns: new[] { "CertificateTypeId", "TaskId" },
                unique: true,
                filter: "[CertificateTypeId] IS NOT NULL AND [TaskId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_TaskId",
                table: "Requirements",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ProjectId",
                table: "Shifts",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_TaskId",
                table: "Shifts",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CategoryId",
                table: "Tasks",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_InstructionId",
                table: "Tasks",
                column: "InstructionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Availabilities");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "Requirements");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "CertificateTypes");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
