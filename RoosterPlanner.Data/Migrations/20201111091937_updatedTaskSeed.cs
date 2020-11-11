using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class updatedTaskSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name", "UrlPdf" },
                values: new object[,]
                {
                    { new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "KEUKEN", "System", new DateTime(2019, 1, 22, 8, 1, 1, 0, DateTimeKind.Unspecified), "Keuken", null },
                    { new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "BEDIENING", "System", new DateTime(2019, 1, 18, 16, 55, 29, 0, DateTimeKind.Unspecified), "Bediening", null },
                    { new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "LOGISTIEK", "System", new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified), "Logistiek", null },
                    { new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "OVERIGE", "System", new DateTime(2019, 1, 15, 2, 22, 55, 0, DateTimeKind.Unspecified), "Overige", null }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Address", "City", "Closed", "Description", "EndDate", "LastEditBy", "LastEditDate", "Name", "PictureUri", "StartDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("852874e2-7b37-44a4-8592-ac064c313aef"), "Stationsplein 2", "Voorburg", false, "Leuk project in Voorburg", new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Voorburg 2020", null, new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("ce3cba97-8fee-4cad-8665-dad2aac17276"), "Laan van Waalhaven 450", "Den Haag", false, "Leuk project in Den Haag", new DateTime(2018, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Den Haag 2018", null, new DateTime(2018, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CategoryId", "Color", "DeletedDateTime", "Description", "DocumentUri", "LastEditBy", "LastEditDate", "Name" },
                values: new object[,]
                {
                    { new Guid("80405368-5098-478e-8d4b-b2f60f4bb3a3"), new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "Blue", null, "Een leuke beschrijving van de werkzaamheden van een chef", "http://test.com/chef", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chef" },
                    { new Guid("10ea4a9f-14a3-4b17-9ef6-b1c19db0a6e9"), new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "Red", null, "Een leuke beschrijving van de werkzaamheden van een runner", "http://test.com/runner", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Runner" },
                    { new Guid("02d45be0-e502-400c-a05c-8dab1f24900f"), new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "Yellow", null, "Een leuke beschrijving van de werkzaamheden van een chauffeur", "http://test.com/chauffeur", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chauffeur" },
                    { new Guid("1cfe83a7-919b-4967-ba49-ea39ace02584"), new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "Green", null, "Een leuke beschrijving van de werkzaamheden van een klusser", "http://test.com/Klusser", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Klusser" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("852874e2-7b37-44a4-8592-ac064c313aef"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("ce3cba97-8fee-4cad-8665-dad2aac17276"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("02d45be0-e502-400c-a05c-8dab1f24900f"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("10ea4a9f-14a3-4b17-9ef6-b1c19db0a6e9"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("1cfe83a7-919b-4967-ba49-ea39ace02584"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("80405368-5098-478e-8d4b-b2f60f4bb3a3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"));

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
    }
}
