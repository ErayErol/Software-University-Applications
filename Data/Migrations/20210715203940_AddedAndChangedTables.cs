using Microsoft.EntityFrameworkCore.Migrations;

namespace MessiFinder.Data.Migrations
{
    public partial class AddedAndChangedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Playgrounds",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Playgrounds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPlayers",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "WithGoalkeeper",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Playgrounds");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Playgrounds");

            migrationBuilder.DropColumn(
                name: "NumberOfPlayers",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "WithGoalkeeper",
                table: "Games");
        }
    }
}
