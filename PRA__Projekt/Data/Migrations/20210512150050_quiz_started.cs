using Microsoft.EntityFrameworkCore.Migrations;

namespace PRA__Projekt.Data.Migrations
{
    public partial class quiz_started : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Started",
                table: "Quizs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Started",
                table: "Quizs");
        }
    }
}
