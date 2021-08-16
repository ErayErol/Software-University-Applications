namespace MiniFootball.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class GamesTableFieldTableIsPublicColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Fields",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Fields");
        }
    }
}
