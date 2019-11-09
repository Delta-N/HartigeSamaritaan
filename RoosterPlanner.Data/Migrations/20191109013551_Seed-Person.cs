using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class SeedPerson : Migration
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
                    Name = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("11ae0153-5855-4147-8f62-46306ca248c7"), "KEUKEN", "System", new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3685), "Keuken" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("b75c14ee-ffcc-4e23-92b2-165027b99c57"), "BEDIENING", "System", new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3688), "Bediening" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("9a2e83ee-eee6-4022-8dd3-c6fb2966600d"), "LOGISTIEK", "System", new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3690), "Logistiek" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "LastEditBy", "LastEditDate", "Name", "Oid" },
                values: new object[] { new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"), "System", new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3881), "Grace Hopper", new Guid("b691f9f7-c404-4d52-a34f-c90702ca7138") });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "LastEditBy", "LastEditDate", "Name", "Oid" },
                values: new object[] { new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"), "System", new DateTime(2019, 11, 9, 1, 35, 51, 850, DateTimeKind.Utc).AddTicks(3894), "John Wick", new Guid("e2a94901-6942-4cfb-83fa-60343c0de219") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"));
        }
    }
}
