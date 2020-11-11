using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class updatedParticipationSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Address", "City", "Closed", "Description", "EndDate", "LastEditBy", "LastEditDate", "Name", "PictureUri", "StartDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("e86bb765-27ab-404f-b140-211505d869fe"), "Stationsplein 2", "Voorburg", false, "Leuk project in Voorburg", new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Voorburg 2020", null, new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"), "Laan van Waalhaven 450", "Den Haag", false, "Leuk project in Den Haag", new DateTime(2018, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Den Haag 2018", null, new DateTime(2018, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CategoryId", "Color", "DeletedDateTime", "Description", "DocumentUri", "LastEditBy", "LastEditDate", "Name" },
                values: new object[,]
                {
                    { new Guid("50756bdb-8bfb-4646-8119-35fbb4cfc8f9"), new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "Blue", null, "Een leuke beschrijving van de werkzaamheden van een chef", "http://test.com/chef", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chef" },
                    { new Guid("17e71d4f-5da4-4146-91bf-069edaf452c2"), new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "Red", null, "Een leuke beschrijving van de werkzaamheden van een runner", "http://test.com/runner", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Runner" },
                    { new Guid("adb41bf9-b0fe-4d2d-8ee5-4040f07a4a6c"), new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "Yellow", null, "Een leuke beschrijving van de werkzaamheden van een chauffeur", "http://test.com/chauffeur", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chauffeur" },
                    { new Guid("3a1185f2-9de9-4401-9963-b859427806b5"), new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "Green", null, "Een leuke beschrijving van de werkzaamheden van een klusser", "http://test.com/Klusser", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Klusser" }
                });

            migrationBuilder.InsertData(
                table: "Participations",
                columns: new[] { "Id", "LastEditBy", "LastEditDate", "MaxWorkingHoursPerWeek", "PersonId", "ProjectId" },
                values: new object[] { new Guid("66e971cf-16f2-4521-befb-aaca981f642f"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"), new Guid("e86bb765-27ab-404f-b140-211505d869fe") });

            migrationBuilder.InsertData(
                table: "Participations",
                columns: new[] { "Id", "LastEditBy", "LastEditDate", "MaxWorkingHoursPerWeek", "PersonId", "ProjectId" },
                values: new object[] { new Guid("541310c7-ffec-43f5-81a7-7b2c07f9ce81"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"), new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Participations",
                keyColumn: "Id",
                keyValue: new Guid("541310c7-ffec-43f5-81a7-7b2c07f9ce81"));

            migrationBuilder.DeleteData(
                table: "Participations",
                keyColumn: "Id",
                keyValue: new Guid("66e971cf-16f2-4521-befb-aaca981f642f"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("17e71d4f-5da4-4146-91bf-069edaf452c2"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("3a1185f2-9de9-4401-9963-b859427806b5"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("50756bdb-8bfb-4646-8119-35fbb4cfc8f9"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("adb41bf9-b0fe-4d2d-8ee5-4040f07a4a6c"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("e86bb765-27ab-404f-b140-211505d869fe"));

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
    }
}
