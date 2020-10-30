using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class ChangedPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2eb2c6fa-bffc-4c8e-83ce-42103da372fc"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3d578493-149c-4e7d-b112-c096f5b8504d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("88eebbcf-11c2-458c-8769-8e6cd3f6a6aa"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("1c52b2e9-aa97-493b-a0e0-0062cb0dead1"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("f7e48ae8-e384-4852-adc6-1769bf622b33"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Persons");

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "Persons",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[,]
                {
                    { new Guid("a469bfe5-69aa-4f1b-8301-28619870c9a3"), "KEUKEN", "System", new DateTime(2019, 1, 22, 8, 1, 1, 0, DateTimeKind.Unspecified), "Keuken", null },
                    { new Guid("499f4ac4-c29d-4165-b914-2ef1303df9cd"), "BEDIENING", "System", new DateTime(2019, 1, 18, 16, 55, 29, 0, DateTimeKind.Unspecified), "Bediening", null },
                    { new Guid("d2736b7b-a6d9-4fb8-b70b-951bc0c03db6"), "LOGISTIEK", "System", new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified), "Logistiek", null }
                });

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                column: "firstName",
                value: "Grace");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                column: "firstName",
                value: "John");

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Address", "City", "Closed", "Description", "EndDate", "LastEditBy", "LastEditDate", "Name", "PictureUri", "StartDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("570f81b2-798a-46be-8885-6561c5ed101e"), "Stationsplein 2", "Voorburg", false, "Leuk project in Voorburg", new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Voorburg 2020", null, new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("8136cb8f-dbc0-4cb7-acef-003233fe9be7"), "Laan van Waalhaven 450", "Den Haag", false, "Leuk project in Den Haag", new DateTime(2018, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Den Haag 2018", null, new DateTime(2018, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("499f4ac4-c29d-4165-b914-2ef1303df9cd"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a469bfe5-69aa-4f1b-8301-28619870c9a3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d2736b7b-a6d9-4fb8-b70b-951bc0c03db6"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("570f81b2-798a-46be-8885-6561c5ed101e"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("8136cb8f-dbc0-4cb7-acef-003233fe9be7"));

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "Persons");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Persons",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[,]
                {
                    { new Guid("88eebbcf-11c2-458c-8769-8e6cd3f6a6aa"), "KEUKEN", "System", new DateTime(2019, 1, 22, 8, 1, 1, 0, DateTimeKind.Unspecified), "Keuken", null },
                    { new Guid("3d578493-149c-4e7d-b112-c096f5b8504d"), "BEDIENING", "System", new DateTime(2019, 1, 18, 16, 55, 29, 0, DateTimeKind.Unspecified), "Bediening", null },
                    { new Guid("2eb2c6fa-bffc-4c8e-83ce-42103da372fc"), "LOGISTIEK", "System", new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified), "Logistiek", null }
                });

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                column: "Name",
                value: "Grace Hopper");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                column: "Name",
                value: "John Wick");

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Address", "City", "Closed", "Description", "EndDate", "LastEditBy", "LastEditDate", "Name", "PictureUri", "StartDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("f7e48ae8-e384-4852-adc6-1769bf622b33"), "Stationsplein 2", "Voorburg", false, "Leuk project in Voorburg", new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Voorburg 2020", null, new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("1c52b2e9-aa97-493b-a0e0-0062cb0dead1"), "Laan van Waalhaven 450", "Den Haag", false, "Leuk project in Den Haag", new DateTime(2018, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Den Haag 2018", null, new DateTime(2018, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }
    }
}
