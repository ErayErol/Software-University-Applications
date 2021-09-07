using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniFootball.Data.Migrations
{
    public partial class ChangeTableNameFromImageUrlToPhotoPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Fields",
                newName: "PhotoPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "Fields",
                newName: "ImageUrl");
        }
    }
}
