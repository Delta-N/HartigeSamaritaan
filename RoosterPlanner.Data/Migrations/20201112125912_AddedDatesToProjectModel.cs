using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class AddedDatesToProjectModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Projects");

            migrationBuilder.AddColumn<DateTime>(
                name: "ParticipationEndDate",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ParticipationStartDate",
                table: "Projects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjectEndDate",
                table: "Projects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjectStartDate",
                table: "Projects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"),
                columns: new[] { "ParticipationEndDate", "ParticipationStartDate", "ProjectEndDate", "ProjectStartDate" },
                values: new object[] { new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("e86bb765-27ab-404f-b140-211505d869fe"),
                columns: new[] { "ParticipationEndDate", "ParticipationStartDate", "ProjectEndDate", "ProjectStartDate" },
                values: new object[] { new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

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
                table: "Tasks",
                columns: new[] { "Id", "CategoryId", "Color", "DeletedDateTime", "Description", "DocumentUri", "LastEditBy", "LastEditDate", "Name" },
                values: new object[,]
                {
                    { new Guid("c722d85c-b3a8-454d-a4d3-93c667c61192"), new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "Blue", null, "Een leuke beschrijving van de werkzaamheden van een chef", "http://test.com/chef", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chef" },
                    { new Guid("0a347fc7-103b-4a4a-bbcc-72779ad04a0d"), new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "Red", null, "Een leuke beschrijving van de werkzaamheden van een runner", "http://test.com/runner", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Runner" },
                    { new Guid("c96d2a5e-e339-4815-91cc-13c5f77579e6"), new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "Yellow", null, "Een leuke beschrijving van de werkzaamheden van een chauffeur", "http://test.com/chauffeur", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chauffeur" },
                    { new Guid("8298e406-67d5-41f6-8b1d-dc0474af8fa3"), new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "Green", null, "Een leuke beschrijving van de werkzaamheden van een klusser", "http://test.com/Klusser", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Klusser" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("0a347fc7-103b-4a4a-bbcc-72779ad04a0d"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("8298e406-67d5-41f6-8b1d-dc0474af8fa3"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("c722d85c-b3a8-454d-a4d3-93c667c61192"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("c96d2a5e-e339-4815-91cc-13c5f77579e6"));

            migrationBuilder.DropColumn(
                name: "ParticipationEndDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ParticipationStartDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectEndDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectStartDate",
                table: "Projects");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2018, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2018, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("e86bb765-27ab-404f-b140-211505d869fe"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2020, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

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
                table: "Tasks",
                columns: new[] { "Id", "CategoryId", "Color", "DeletedDateTime", "Description", "DocumentUri", "LastEditBy", "LastEditDate", "Name" },
                values: new object[,]
                {
                    { new Guid("50756bdb-8bfb-4646-8119-35fbb4cfc8f9"), new Guid("bd065d8a-c6f2-4ec5-84fd-92636f52f309"), "Blue", null, "Een leuke beschrijving van de werkzaamheden van een chef", "http://test.com/chef", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chef" },
                    { new Guid("17e71d4f-5da4-4146-91bf-069edaf452c2"), new Guid("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), "Red", null, "Een leuke beschrijving van de werkzaamheden van een runner", "http://test.com/runner", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Runner" },
                    { new Guid("adb41bf9-b0fe-4d2d-8ee5-4040f07a4a6c"), new Guid("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"), "Yellow", null, "Een leuke beschrijving van de werkzaamheden van een chauffeur", "http://test.com/chauffeur", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chauffeur" },
                    { new Guid("3a1185f2-9de9-4401-9963-b859427806b5"), new Guid("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), "Green", null, "Een leuke beschrijving van de werkzaamheden van een klusser", "http://test.com/Klusser", "SYSTEM", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Klusser" }
                });
        }
    }
}
