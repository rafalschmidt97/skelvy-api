using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skelvy.Persistence.Migrations
{
    public partial class RenameFriendRequestToInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.CreateTable(
                name: "FriendInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InvitingUserId = table.Column<int>(nullable: false),
                    InvitedUserId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 15, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendInvitations_Users_InvitedUserId",
                        column: x => x.InvitedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendInvitations_Users_InvitingUserId",
                        column: x => x.InvitingUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendInvitations_InvitedUserId",
                table: "FriendInvitations",
                column: "InvitedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendInvitations_InvitingUserId",
                table: "FriendInvitations",
                column: "InvitingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendInvitations_IsRemoved",
                table: "FriendInvitations",
                column: "IsRemoved");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendInvitations");

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    InvitedUserId = table.Column<int>(nullable: false),
                    InvitingUserId = table.Column<int>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    Status = table.Column<string>(maxLength: 15, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequests_Users_InvitedUserId",
                        column: x => x.InvitedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequests_Users_InvitingUserId",
                        column: x => x.InvitingUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_InvitedUserId",
                table: "FriendRequests",
                column: "InvitedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_InvitingUserId",
                table: "FriendRequests",
                column: "InvitingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_IsRemoved",
                table: "FriendRequests",
                column: "IsRemoved");
        }
    }
}
