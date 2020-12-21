using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class fresh : Migration
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
                    UrlPdf = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Level = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PersonalRemark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    StaffRemark = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
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
                    PictureUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    DocumentUri = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateExpired = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Requirements_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[,]
                {
                    { new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "KEUKEN", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 712, DateTimeKind.Local).AddTicks(9887), "Keuken", null },
                    { new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "BEDIENING", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 713, DateTimeKind.Local).AddTicks(173), "Bediening", null },
                    { new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "LOGISTIEK", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 713, DateTimeKind.Local).AddTicks(188), "Logistiek", null },
                    { new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "OVERIGE", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 713, DateTimeKind.Local).AddTicks(193), "Overige", null }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "FirstName", "LastEditBy", "LastEditDate", "LastName", "Oid", "PersonalRemark", "StaffRemark", "Type" },
                values: new object[,]
                {
                    { new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"), "Grace", "System", new DateTime(2019, 1, 1, 12, 34, 28, 0, DateTimeKind.Unspecified), null, new Guid("b691f9f7-c404-4d52-a34f-c90702ca7138"), null, null, 0 },
                    { new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"), "John", "System", new DateTime(2019, 1, 2, 12, 45, 1, 0, DateTimeKind.Unspecified), null, new Guid("e2a94901-6942-4cfb-83fa-60343c0de219"), null, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Address", "City", "Closed", "Description", "LastEditBy", "LastEditDate", "Name", "ParticipationEndDate", "ParticipationStartDate", "PictureUri", "ProjectEndDate", "ProjectStartDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("e86bb765-27ab-404f-b140-211505d869fe"), "Stationsplein 2", "Voorburg", false, "Leuk project in Voorburg", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(8589), "Voorburg 2020", new DateTime(2021, 1, 19, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(7925), new DateTime(2020, 12, 22, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(7659), null, new DateTime(2021, 1, 20, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(8397), new DateTime(2020, 12, 21, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(8177), null },
                    { new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"), "Laan van Waalhaven 450", "Den Haag", false, "Leuk project in Den Haag", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(8634), "Den Haag 2018", new DateTime(2021, 1, 19, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(8627), new DateTime(2020, 12, 22, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(8622), null, new DateTime(2021, 1, 20, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(8632), new DateTime(2020, 12, 21, 15, 2, 4, 714, DateTimeKind.Local).AddTicks(8629), null }
                });

            migrationBuilder.InsertData(
                table: "Participations",
                columns: new[] { "Id", "Active", "LastEditBy", "LastEditDate", "MaxWorkingHoursPerWeek", "PersonId", "ProjectId" },
                values: new object[,]
                {
                    { new Guid("66e971cf-16f2-4521-befb-aaca981f642f"), true, "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 715, DateTimeKind.Local).AddTicks(2874), 12, new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"), new Guid("e86bb765-27ab-404f-b140-211505d869fe") },
                    { new Guid("541310c7-ffec-43f5-81a7-7b2c07f9ce81"), true, "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 715, DateTimeKind.Local).AddTicks(2912), 40, new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"), new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a") }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CategoryId", "Color", "DeletedDateTime", "Description", "DocumentUri", "LastEditBy", "LastEditDate", "Name" },
                values: new object[,]
                {
                    { new Guid("f888bac5-166b-4621-b322-f162e0123d5c"), new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "Blue", null, "Een leuke beschrijving van de werkzaamheden van een chef", "https://hackatonstoragedev.blob.core.windows.net/instructiondocuments/cf433ca6-0de4-4bec-b270-e5d4ef19b599.txt", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 715, DateTimeKind.Local).AddTicks(9149), "Chef" },
                    { new Guid("78c2fbb4-5793-4054-a078-5011766c3242"), new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "Red", null, "Een leuke beschrijving van de werkzaamheden van een runner", "https://hackatonstoragedev.blob.core.windows.net/instructiondocuments/cf433ca6-0de4-4bec-b270-e5d4ef19b599.txt", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 715, DateTimeKind.Local).AddTicks(9190), "Runner" },
                    { new Guid("0ec6a73c-0e29-4f51-96b0-419f68d0bb9c"), new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "Yellow", null, "Een leuke beschrijving van de werkzaamheden van een chauffeur", "https://hackatonstoragedev.blob.core.windows.net/instructiondocuments/cf433ca6-0de4-4bec-b270-e5d4ef19b599.txt", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 715, DateTimeKind.Local).AddTicks(9201), "Chauffeur" },
                    { new Guid("340a82dc-b193-4229-b080-b98171e4fb90"), new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "Green", null, "Een leuke beschrijving van de werkzaamheden van een klusser", "https://hackatonstoragedev.blob.core.windows.net/instructiondocuments/cf433ca6-0de4-4bec-b270-e5d4ef19b599.txt", "SYSTEM", new DateTime(2020, 12, 21, 15, 2, 4, 715, DateTimeKind.Local).AddTicks(9218), "Klusser" }
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
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_TaskId",
                table: "ProjectTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_CertificateTypeId",
                table: "Requirements",
                column: "CertificateTypeId");

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
        }
    }
}
