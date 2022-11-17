using Microsoft.EntityFrameworkCore.Migrations;

namespace PRA__Projekt.Data.Migrations
{
    public partial class question_ended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ended",
                table: "Questions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ended",
                table: "Questions");
        }
    }
}
