using Microsoft.EntityFrameworkCore.Migrations;

namespace Skelvy.Persistence.Migrations
{
    public partial class AddUserProfilePhotoOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "UserProfilePhotos",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "UserProfilePhotos");
        }
    }
}
