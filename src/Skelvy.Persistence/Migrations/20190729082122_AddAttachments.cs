using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skelvy.Persistence.Migrations
{
    public partial class AddAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "UserProfilePhotos");

            migrationBuilder.DropColumn(
                name: "AttachmentUrl",
                table: "MeetingChatMessages");

            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "UserProfilePhotos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "MeetingChatMessages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    Url = table.Column<string>(maxLength: 2048, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfilePhotos_AttachmentId",
                table: "UserProfilePhotos",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingChatMessages_AttachmentId",
                table: "MeetingChatMessages",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingChatMessages_Attachments_AttachmentId",
                table: "MeetingChatMessages",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfilePhotos_Attachments_AttachmentId",
                table: "UserProfilePhotos",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingChatMessages_Attachments_AttachmentId",
                table: "MeetingChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfilePhotos_Attachments_AttachmentId",
                table: "UserProfilePhotos");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_UserProfilePhotos_AttachmentId",
                table: "UserProfilePhotos");

            migrationBuilder.DropIndex(
                name: "IX_MeetingChatMessages_AttachmentId",
                table: "MeetingChatMessages");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "UserProfilePhotos");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "MeetingChatMessages");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "UserProfilePhotos",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentUrl",
                table: "MeetingChatMessages",
                maxLength: 2048,
                nullable: true);
        }
    }
}
