using Microsoft.EntityFrameworkCore.Migrations;

namespace Vehicles.API.Migrations
{
    public partial class UpdateTableProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Procedimientos",
                table: "Procedimientos");

            migrationBuilder.RenameTable(
                name: "Procedimientos",
                newName: "Procedures");

            migrationBuilder.RenameIndex(
                name: "IX_Procedimientos_Description",
                table: "Procedures",
                newName: "IX_Procedures_Description");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Procedures",
                table: "Procedures",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Procedures",
                table: "Procedures");

            migrationBuilder.RenameTable(
                name: "Procedures",
                newName: "Procedimientos");

            migrationBuilder.RenameIndex(
                name: "IX_Procedures_Description",
                table: "Procedimientos",
                newName: "IX_Procedimientos_Description");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Procedimientos",
                table: "Procedimientos",
                column: "Id");
        }
    }
}
