using Microsoft.EntityFrameworkCore.Migrations;

namespace PRA__Projekt.Data.Migrations
{
    public partial class quiz_ended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ended",
                table: "Quizs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ended",
                table: "Quizs");
        }
    }
}
