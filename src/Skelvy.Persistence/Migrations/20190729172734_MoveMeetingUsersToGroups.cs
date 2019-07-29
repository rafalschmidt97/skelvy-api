using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skelvy.Persistence.Migrations
{
    public partial class MoveMeetingUsersToGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingChatMessages_Meetings_MeetingId",
                table: "MeetingChatMessages");

            migrationBuilder.DropTable(
                name: "MeetingUsers");

            migrationBuilder.RenameColumn(
                name: "MeetingId",
                table: "MeetingChatMessages",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingChatMessages_MeetingId",
                table: "MeetingChatMessages",
                newName: "IX_MeetingChatMessages_GroupId");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Meetings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    RemovedReason = table.Column<string>(maxLength: 15, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    RemovedReason = table.Column<string>(maxLength: 15, nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    MeetingRequestId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupUsers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupUsers_MeetingRequests_MeetingRequestId",
                        column: x => x.MeetingRequestId,
                        principalTable: "MeetingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_GroupId",
                table: "Meetings",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_IsRemoved",
                table: "Groups",
                column: "IsRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupId",
                table: "GroupUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_IsRemoved",
                table: "GroupUsers",
                column: "IsRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_MeetingRequestId",
                table: "GroupUsers",
                column: "MeetingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_UserId",
                table: "GroupUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingChatMessages_Groups_GroupId",
                table: "MeetingChatMessages",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Groups_GroupId",
                table: "Meetings",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingChatMessages_Groups_GroupId",
                table: "MeetingChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Groups_GroupId",
                table: "Meetings");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_GroupId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Meetings");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "MeetingChatMessages",
                newName: "MeetingId");

            migrationBuilder.RenameIndex(
                name: "IX_MeetingChatMessages_GroupId",
                table: "MeetingChatMessages",
                newName: "IX_MeetingChatMessages_MeetingId");

            migrationBuilder.CreateTable(
                name: "MeetingUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    MeetingId = table.Column<int>(nullable: false),
                    MeetingRequestId = table.Column<int>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    RemovedReason = table.Column<string>(maxLength: 15, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetingUsers_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingUsers_MeetingRequests_MeetingRequestId",
                        column: x => x.MeetingRequestId,
                        principalTable: "MeetingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingUsers_IsRemoved",
                table: "MeetingUsers",
                column: "IsRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingUsers_MeetingId",
                table: "MeetingUsers",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingUsers_MeetingRequestId",
                table: "MeetingUsers",
                column: "MeetingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingUsers_UserId",
                table: "MeetingUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingChatMessages_Meetings_MeetingId",
                table: "MeetingChatMessages",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
