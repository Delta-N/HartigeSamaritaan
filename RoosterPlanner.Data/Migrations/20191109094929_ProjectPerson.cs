using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class ProjectPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("65aef8f8-1c7e-4fd8-834b-f3eb622b8c89"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c661cf62-0c96-4c0e-b9bd-2f5b9de04034"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("fab511b4-fa8e-400d-939d-f03a87830976"));

            migrationBuilder.CreateTable(
                name: "ProjectPerson",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    LastEditBy = table.Column<string>(maxLength: 128, nullable: true),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPerson", x => new { x.ProjectId, x.PersonId });
                    table.UniqueConstraint("AK_ProjectPerson_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPerson_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectPerson_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("e1e4a380-029b-4950-a1e9-9c5e936c5ccc"), "KEUKEN", "System", new DateTime(2019, 11, 9, 9, 49, 28, 878, DateTimeKind.Utc).AddTicks(7963), "Keuken" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("a00f6de4-dcda-4ac9-8e75-086fd0d21097"), "BEDIENING", "System", new DateTime(2019, 11, 9, 9, 49, 28, 878, DateTimeKind.Utc).AddTicks(7967), "Bediening" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("b6a0b994-4a33-40c5-b113-84b9aa748141"), "LOGISTIEK", "System", new DateTime(2019, 11, 9, 9, 49, 28, 878, DateTimeKind.Utc).AddTicks(7977), "Logistiek" });

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                column: "LastEditDate",
                value: new DateTime(2019, 11, 9, 9, 49, 28, 878, DateTimeKind.Utc).AddTicks(8249));

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                column: "LastEditDate",
                value: new DateTime(2019, 11, 9, 9, 49, 28, 878, DateTimeKind.Utc).AddTicks(8264));

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Oid",
                table: "Persons",
                column: "Oid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPerson_PersonId",
                table: "ProjectPerson",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectPerson");

            migrationBuilder.DropIndex(
                name: "IX_Persons_Oid",
                table: "Persons");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a00f6de4-dcda-4ac9-8e75-086fd0d21097"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b6a0b994-4a33-40c5-b113-84b9aa748141"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e1e4a380-029b-4950-a1e9-9c5e936c5ccc"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("c661cf62-0c96-4c0e-b9bd-2f5b9de04034"), "KEUKEN", "System", new DateTime(2019, 11, 9, 1, 37, 44, 486, DateTimeKind.Utc).AddTicks(2848), "Keuken" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("fab511b4-fa8e-400d-939d-f03a87830976"), "BEDIENING", "System", new DateTime(2019, 11, 9, 1, 37, 44, 486, DateTimeKind.Utc).AddTicks(2852), "Bediening" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("65aef8f8-1c7e-4fd8-834b-f3eb622b8c89"), "LOGISTIEK", "System", new DateTime(2019, 11, 9, 1, 37, 44, 486, DateTimeKind.Utc).AddTicks(2854), "Logistiek" });

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                column: "LastEditDate",
                value: new DateTime(2019, 11, 9, 7, 4, 59, 461, DateTimeKind.Utc).AddTicks(4156));

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                column: "LastEditDate",
                value: new DateTime(2019, 11, 9, 7, 4, 59, 461, DateTimeKind.Utc).AddTicks(4168));
        }
    }
}
