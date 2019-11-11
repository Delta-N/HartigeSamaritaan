using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class SeedPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
