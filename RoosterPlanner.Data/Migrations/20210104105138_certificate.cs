using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class certificate : Migration
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
                        onDelete: ReferentialAction.SetNull);
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
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[,]
                {
                    { new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "KEUKEN", "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 110, DateTimeKind.Local).AddTicks(8114), "Keuken" },
                    { new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "BEDIENING", "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 110, DateTimeKind.Local).AddTicks(8418), "Bediening" },
                    { new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "LOGISTIEK", "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 110, DateTimeKind.Local).AddTicks(8434), "Logistiek" },
                    { new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "OVERIGE", "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 110, DateTimeKind.Local).AddTicks(8440), "Overige" }
                });

            migrationBuilder.InsertData(
                table: "CertificateTypes",
                columns: new[] { "Id", "LastEditBy", "LastEditDate", "Level", "Name" },
                values: new object[,]
                {
                    { new Guid("3a1a7954-c0c9-4a4f-b1e4-ab479b6dce76"), "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 113, DateTimeKind.Local).AddTicks(8872), null, "Biefstuk-capable" },
                    { new Guid("98b3d87c-89b9-4866-9a0b-8e3269fa8183"), "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 113, DateTimeKind.Local).AddTicks(9116), "B", "Rijbewijs" },
                    { new Guid("b16a0b16-c0dd-43d7-b6c7-9853c0a6229d"), "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 113, DateTimeKind.Local).AddTicks(9132), null, "Barcertificaat" },
                    { new Guid("87719cc4-8985-4860-9294-b2f1c2337376"), "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 113, DateTimeKind.Local).AddTicks(9143), null, "HACCP" }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "DocumentUri", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("a8b01392-b3d7-4999-815e-2deee8714447"), "https://hackatonstoragedev.blob.core.windows.net/projectpicture/037efc9a-0836-4513-8ae3-282c597631c4.jfif", "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 113, DateTimeKind.Local).AddTicks(4926), "TermsOfService" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Address", "City", "Closed", "Description", "LastEditBy", "LastEditDate", "Name", "ParticipationEndDate", "ParticipationStartDate", "PictureUriId", "ProjectEndDate", "ProjectStartDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("e86bb765-27ab-404f-b140-211505d869fe"), "Stationsplein 2", "Voorburg", false, "Leuk project in Voorburg", "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(4279), "Voorburg 2020", new DateTime(2021, 2, 2, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(3596), new DateTime(2021, 1, 5, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(3311), null, new DateTime(2021, 2, 3, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(4079), new DateTime(2021, 1, 4, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(3855), null },
                    { new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"), "Laan van Waalhaven 450", "Den Haag", false, "Leuk project in Den Haag", "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(4325), "Den Haag 2018", new DateTime(2021, 2, 2, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(4318), new DateTime(2021, 1, 5, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(4313), null, new DateTime(2021, 2, 3, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(4323), new DateTime(2021, 1, 4, 11, 51, 37, 112, DateTimeKind.Local).AddTicks(4320), null }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CategoryId", "Color", "Description", "InstructionId", "LastEditBy", "LastEditDate", "Name" },
                values: new object[,]
                {
                    { new Guid("bcc9a242-8505-493c-a47a-18b703701ae6"), new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "Blue", "Een leuke beschrijving van de werkzaamheden van een chef", null, "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 113, DateTimeKind.Local).AddTicks(1208), "Chef" },
                    { new Guid("2039730b-b4fd-4e47-addf-1d4dd50cfaf6"), new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "Red", "Een leuke beschrijving van de werkzaamheden van een runner", null, "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 113, DateTimeKind.Local).AddTicks(1311), "Runner" },
                    { new Guid("02f92fb5-7257-4814-8427-85bcc8c4bea9"), new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "Yellow", "Een leuke beschrijving van de werkzaamheden van een chauffeur", null, "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 113, DateTimeKind.Local).AddTicks(1323), "Chauffeur" },
                    { new Guid("fd8d3cc5-ba72-420b-b358-871b1fee76e8"), new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "Green", "Een leuke beschrijving van de werkzaamheden van een klusser", null, "SYSTEM", new DateTime(2021, 1, 4, 11, 51, 37, 113, DateTimeKind.Local).AddTicks(1330), "Klusser" }
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
