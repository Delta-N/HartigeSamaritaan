using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class TaskmodelURI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("0ca10a56-cf22-4afe-a56b-a56a4c496631"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("202b5144-8232-4531-a080-60cca12919d1"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("b4394291-861d-4ed8-b13e-9b4d5f105dc5"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("ce45a6a5-a41a-4fa5-887f-d67b13fbad94"));

            migrationBuilder.AlterColumn<string>(
                name: "DocumentUri",
                table: "Tasks",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("2354a5ab-56a7-4e0e-91e0-a5fb34145624"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("58b28157-e4a0-48a2-8813-35c57187b56a"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("7e62a962-68a8-4d3c-ac50-9a3320173c48"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("b1c6e7ed-78cf-435b-9f1f-576bb0ca7934"));

            migrationBuilder.AlterColumn<string>(
                name: "DocumentUri",
                table: "Tasks",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
