using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class ModelRework2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("00e901a9-b341-46ce-8a8d-28233d1b6b04"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("01061dc4-d872-49d0-9fa4-b687d18359e4"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e144dda4-267c-4864-b1bb-635bd8967977"));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[,]
                {
                    { new Guid("88eebbcf-11c2-458c-8769-8e6cd3f6a6aa"), "KEUKEN", "System", new DateTime(2019, 1, 22, 8, 1, 1, 0, DateTimeKind.Unspecified), "Keuken", null },
                    { new Guid("3d578493-149c-4e7d-b112-c096f5b8504d"), "BEDIENING", "System", new DateTime(2019, 1, 18, 16, 55, 29, 0, DateTimeKind.Unspecified), "Bediening", null },
                    { new Guid("2eb2c6fa-bffc-4c8e-83ce-42103da372fc"), "LOGISTIEK", "System", new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified), "Logistiek", null }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Address", "City", "Closed", "Description", "EndDate", "LastEditBy", "LastEditDate", "Name", "PictureUri", "StartDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("f7e48ae8-e384-4852-adc6-1769bf622b33"), "Stationsplein 2", "Voorburg", false, "Leuk project in Voorburg", new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Voorburg 2020", null, new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("1c52b2e9-aa97-493b-a0e0-0062cb0dead1"), "Laan van Waalhaven 450", "Den Haag", false, "Leuk project in Den Haag", new DateTime(2018, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Den Haag 2018", null, new DateTime(2018, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[] { new Guid("e144dda4-267c-4864-b1bb-635bd8967977"), "KEUKEN", "System", new DateTime(2019, 1, 22, 8, 1, 1, 0, DateTimeKind.Unspecified), "Keuken", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[] { new Guid("00e901a9-b341-46ce-8a8d-28233d1b6b04"), "BEDIENING", "System", new DateTime(2019, 1, 18, 16, 55, 29, 0, DateTimeKind.Unspecified), "Bediening", null });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[] { new Guid("01061dc4-d872-49d0-9fa4-b687d18359e4"), "LOGISTIEK", "System", new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified), "Logistiek", null });
        }
    }
}
