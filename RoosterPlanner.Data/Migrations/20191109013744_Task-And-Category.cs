using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoosterPlanner.Data.Migrations
{
    public partial class TaskAndCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Tasks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Tasks",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentUri",
                table: "Tasks",
                maxLength: 128,
                nullable: true);

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
                values: new object[] { new Guid("c661cf62-0c96-4c0e-b9bd-2f5b9de04034"), "KEUKEN", "System", new DateTime(2019, 11, 9, 1, 37, 44, 486, DateTimeKind.Utc).AddTicks(2848), "Keuken" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("fab511b4-fa8e-400d-939d-f03a87830976"), "BEDIENING", "System", new DateTime(2019, 11, 9, 1, 37, 44, 486, DateTimeKind.Utc).AddTicks(2852), "Bediening" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Code", "LastEditBy", "LastEditDate", "Name" },
                values: new object[] { new Guid("65aef8f8-1c7e-4fd8-834b-f3eb622b8c89"), "LOGISTIEK", "System", new DateTime(2019, 11, 9, 1, 37, 44, 486, DateTimeKind.Utc).AddTicks(2854), "Logistiek" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CategoryId",
                table: "Tasks",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Categories_CategoryId",
                table: "Tasks",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Categories_CategoryId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CategoryId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DocumentUri",
                table: "Tasks");
        }
    }
}
