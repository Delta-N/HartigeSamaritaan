using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class ShiftDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Tasks_TaskId",
                table: "Shift");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskId",
                table: "Shift",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Shift",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("58a2bb8c-5e0e-4b24-8cec-adcd1b5654de"), "KEUKEN", "System", new DateTime(2019, 11, 9, 9, 7, 39, 773, DateTimeKind.Utc).AddTicks(5168), "Keuken" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("9c4835cf-221a-42c5-9c7c-82dc83265aa8"), "BEDIENING", "System", new DateTime(2019, 11, 9, 9, 7, 39, 773, DateTimeKind.Utc).AddTicks(5172), "Bediening" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("aa5b1f28-672b-4dee-a73f-d6e4b4e31937"), "LOGISTIEK", "System", new DateTime(2019, 11, 9, 9, 7, 39, 773, DateTimeKind.Utc).AddTicks(5173), "Logistiek" });

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("25e5b0e6-82ef-45fe-bbde-ef76021ec531"),
                column: "LastEditDate",
                value: new DateTime(2019, 11, 9, 9, 7, 39, 773, DateTimeKind.Utc).AddTicks(5376));

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("7f66fc12-b1c0-481f-851b-3cc1f65fd20e"),
                column: "LastEditDate",
                value: new DateTime(2019, 11, 9, 9, 7, 39, 773, DateTimeKind.Utc).AddTicks(5389));

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Tasks_TaskId",
                table: "Shift",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Tasks_TaskId",
                table: "Shift");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("58a2bb8c-5e0e-4b24-8cec-adcd1b5654de"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9c4835cf-221a-42c5-9c7c-82dc83265aa8"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aa5b1f28-672b-4dee-a73f-d6e4b4e31937"));

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Shift");

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskId",
                table: "Shift",
                nullable: true,
                oldClrType: typeof(Guid));

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

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Tasks_TaskId",
                table: "Shift",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
