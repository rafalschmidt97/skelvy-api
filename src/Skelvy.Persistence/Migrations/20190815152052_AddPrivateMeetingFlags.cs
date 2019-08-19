using Microsoft.EntityFrameworkCore.Migrations;

namespace Skelvy.Persistence.Migrations
{
    public partial class AddPrivateMeetingFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Meetings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Meetings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Distance",
                table: "Activities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Activities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Activities",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_IsHidden",
                table: "Meetings",
                column: "IsHidden");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_IsPrivate",
                table: "Meetings",
                column: "IsPrivate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Meetings_IsHidden",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_IsPrivate",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Activities");
        }
    }
}
