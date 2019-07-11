using Microsoft.EntityFrameworkCore.Migrations;

namespace Skelvy.Persistence.Migrations
{
    public partial class AddChatMessageAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrinkId",
                table: "Meetings");

            migrationBuilder.AlterColumn<int>(
                name: "DrinkTypeId",
                table: "Meetings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentUrl",
                table: "MeetingChatMessages",
                maxLength: 2048,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentUrl",
                table: "MeetingChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "DrinkTypeId",
                table: "Meetings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "DrinkId",
                table: "Meetings",
                nullable: false,
                defaultValue: 0);
        }
    }
}
