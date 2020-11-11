using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class updatedTaskModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tasks",
                maxLength: 256,
                nullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[,]
                {
                    { new Guid("c1fb76ad-bf24-404b-8764-fcd3eeef6091"), "KEUKEN", "System", new DateTime(2019, 1, 22, 8, 1, 1, 0, DateTimeKind.Unspecified), "Keuken", null },
                    { new Guid("888cd4c0-73a3-40ed-8120-b880105b6dee"), "BEDIENING", "System", new DateTime(2019, 1, 18, 16, 55, 29, 0, DateTimeKind.Unspecified), "Bediening", null },
                    { new Guid("4427e3c7-82bc-4209-9968-67dbf16f3577"), "LOGISTIEK", "System", new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified), "Logistiek", null }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Address", "City", "Closed", "Description", "EndDate", "LastEditBy", "LastEditDate", "Name", "PictureUri", "StartDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("ac3a8e71-1134-44d6-807a-d19084f96d80"), "Stationsplein 2", "Voorburg", false, "Leuk project in Voorburg", new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Voorburg 2020", null, new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("834b6663-3ae7-4ebb-bcd1-1f4a54bfe4ca"), "Laan van Waalhaven 450", "Den Haag", false, "Leuk project in Den Haag", new DateTime(2018, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Den Haag 2018", null, new DateTime(2018, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4427e3c7-82bc-4209-9968-67dbf16f3577"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("888cd4c0-73a3-40ed-8120-b880105b6dee"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c1fb76ad-bf24-404b-8764-fcd3eeef6091"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("834b6663-3ae7-4ebb-bcd1-1f4a54bfe4ca"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("ac3a8e71-1134-44d6-807a-d19084f96d80"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tasks");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[,]
                {
                    { new Guid("a469bfe5-69aa-4f1b-8301-28619870c9a3"), "KEUKEN", "System", new DateTime(2019, 1, 22, 8, 1, 1, 0, DateTimeKind.Unspecified), "Keuken", null },
                    { new Guid("499f4ac4-c29d-4165-b914-2ef1303df9cd"), "BEDIENING", "System", new DateTime(2019, 1, 18, 16, 55, 29, 0, DateTimeKind.Unspecified), "Bediening", null },
                    { new Guid("d2736b7b-a6d9-4fb8-b70b-951bc0c03db6"), "LOGISTIEK", "System", new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified), "Logistiek", null }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Address", "City", "Closed", "Description", "EndDate", "LastEditBy", "LastEditDate", "Name", "PictureUri", "StartDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("570f81b2-798a-46be-8885-6561c5ed101e"), "Stationsplein 2", "Voorburg", false, "Leuk project in Voorburg", new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Voorburg 2020", null, new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("8136cb8f-dbc0-4cb7-acef-003233fe9be7"), "Laan van Waalhaven 450", "Den Haag", false, "Leuk project in Den Haag", new DateTime(2018, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Den Haag 2018", null, new DateTime(2018, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }
    }
}
