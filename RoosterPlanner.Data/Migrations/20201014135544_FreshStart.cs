using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class FreshStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Code = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    UrlPdf = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Level = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Oid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Address = table.Column<string>(maxLength: 64, nullable: true),
                    City = table.Column<string>(maxLength: 64, nullable: true),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    PictureUri = table.Column<string>(nullable: true),
                    WebsiteUrl = table.Column<string>(nullable: true),
                    Closed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    DeletedDateTime = table.Column<DateTime>(nullable: true),
                    CategoryId = table.Column<Guid>(nullable: false),
                    Color = table.Column<string>(maxLength: 12, nullable: true),
                    DocumentUri = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateExpired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonId = table.Column<Guid>(nullable: false),
                    CertificateTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_CertificateTypes_CertificateTypeId",
                        column: x => x.CertificateTypeId,
                        principalTable: "CertificateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certificates_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    PersonId = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    MaxWorkingHoursPerWeek = table.Column<int>(nullable: false)
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
                    ProjectId = table.Column<Guid>(nullable: false),
                    TaskId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTasks", x => new { x.ProjectId, x.TaskId });
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
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CertificateTypeId = table.Column<Guid>(nullable: false),
                    TaskId = table.Column<Guid>(nullable: false)
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
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    TaskId = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collaborations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    WantsToWorkWithId = table.Column<Guid>(nullable: false),
                    IsWantedById = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collaborations_Participations_IsWantedById",
                        column: x => x.IsWantedById,
                        principalTable: "Participations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Collaborations_Participations_WantsToWorkWithId",
                        column: x => x.WantsToWorkWithId,
                        principalTable: "Participations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ParticipationId = table.Column<Guid>(nullable: false),
                    ShiftId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Preference = table.Column<bool>(nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[,]
                {
                    { new Guid("80eab9f6-26d0-4ac3-a37b-a7eb839145de"), "KEUKEN", "System", new DateTime(2019, 1, 22, 8, 1, 1, 0, DateTimeKind.Unspecified), "Keuken", null },
                    { new Guid("a453da6f-f19b-4f37-b319-165edab40e35"), "BEDIENING", "System", new DateTime(2019, 1, 18, 16, 55, 29, 0, DateTimeKind.Unspecified), "Bediening", null },
                    { new Guid("5096e551-263d-4134-af81-ba71f6c1080a"), "LOGISTIEK", "System", new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified), "Logistiek", null }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "LastEditBy", "LastEditDate", "Name", "Oid", "Type" },
                values: new object[,]
                {
                    { new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"), "System", new DateTime(2019, 1, 1, 12, 34, 28, 0, DateTimeKind.Unspecified), "Grace Hopper", new Guid("b691f9f7-c404-4d52-a34f-c90702ca7138"), 0 },
                    { new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"), "System", new DateTime(2019, 1, 2, 12, 45, 1, 0, DateTimeKind.Unspecified), "John Wick", new Guid("e2a94901-6942-4cfb-83fa-60343c0de219"), 0 }
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
                name: "IX_Collaborations_IsWantedById",
                table: "Collaborations",
                column: "IsWantedById");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborations_WantsToWorkWithId",
                table: "Collaborations",
                column: "WantsToWorkWithId");

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
                name: "Collaborations");

            migrationBuilder.DropTable(
                name: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "Requirements");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Participations");

            migrationBuilder.DropTable(
                name: "CertificateTypes");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
