using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skelvy.Persistence.Migrations
{
    public partial class InitialV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<string>(maxLength: 15, nullable: false),
                    Size = table.Column<int>(nullable: false),
                    Distance = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 15, nullable: false),
                    Url = table.Column<string>(maxLength: 2048, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Language = table.Column<string>(maxLength: 15, nullable: false),
                    FacebookId = table.Column<string>(maxLength: 50, nullable: true),
                    GoogleId = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    ForgottenAt = table.Column<DateTimeOffset>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    DisabledReason = table.Column<string>(maxLength: 1024, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    IsPrivate = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    RemovedReason = table.Column<string>(maxLength: 15, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meetings_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Meetings_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "MeetingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(maxLength: 15, nullable: false),
                    MinDate = table.Column<DateTimeOffset>(nullable: false),
                    MaxDate = table.Column<DateTimeOffset>(nullable: false),
                    MinAge = table.Column<int>(nullable: false),
                    MaxAge = table.Column<int>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    RemovedReason = table.Column<string>(maxLength: 15, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetingRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 15, nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Text = table.Column<string>(maxLength: 500, nullable: true),
                    AttachmentId = table.Column<int>(nullable: true),
                    Action = table.Column<string>(maxLength: 15, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Birthday = table.Column<DateTimeOffset>(nullable: false),
                    Gender = table.Column<string>(maxLength: 15, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Relations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    RelatedUserId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 15, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relations_Users_RelatedUserId",
                        column: x => x.RelatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 15, nullable: false),
                    UserId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MeetingInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InvitingUserId = table.Column<int>(nullable: false),
                    InvitedUserId = table.Column<int>(nullable: false),
                    MeetingId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 15, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetingInvitations_Users_InvitedUserId",
                        column: x => x.InvitedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingInvitations_Users_InvitingUserId",
                        column: x => x.InvitingUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingInvitations_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    MeetingRequestId = table.Column<int>(nullable: true),
                    Role = table.Column<string>(maxLength: 15, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    RemovedReason = table.Column<string>(maxLength: 15, nullable: true),
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

            migrationBuilder.CreateTable(
                name: "MeetingRequestActivities",
                columns: table => new
                {
                    MeetingRequestId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRequestActivities", x => new { x.MeetingRequestId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_MeetingRequestActivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingRequestActivities_MeetingRequests_MeetingRequestId",
                        column: x => x.MeetingRequestId,
                        principalTable: "MeetingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePhotos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttachmentId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilePhotos_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfilePhotos_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Name",
                table: "Activities",
                column: "Name",
                unique: true);

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
                name: "IX_GroupUsers_Role",
                table: "GroupUsers",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_UserId",
                table: "GroupUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingInvitations_InvitedUserId",
                table: "MeetingInvitations",
                column: "InvitedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingInvitations_InvitingUserId",
                table: "MeetingInvitations",
                column: "InvitingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingInvitations_IsRemoved",
                table: "MeetingInvitations",
                column: "IsRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingInvitations_MeetingId",
                table: "MeetingInvitations",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequestActivities_ActivityId",
                table: "MeetingRequestActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequests_IsRemoved",
                table: "MeetingRequests",
                column: "IsRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequests_Latitude",
                table: "MeetingRequests",
                column: "Latitude");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequests_Longitude",
                table: "MeetingRequests",
                column: "Longitude");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequests_MaxAge",
                table: "MeetingRequests",
                column: "MaxAge");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequests_MaxDate",
                table: "MeetingRequests",
                column: "MaxDate");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequests_MinAge",
                table: "MeetingRequests",
                column: "MinAge");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequests_MinDate",
                table: "MeetingRequests",
                column: "MinDate");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequests_Status",
                table: "MeetingRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequests_UserId",
                table: "MeetingRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ActivityId",
                table: "Meetings",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_Date",
                table: "Meetings",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_GroupId",
                table: "Meetings",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_IsHidden",
                table: "Meetings",
                column: "IsHidden");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_IsPrivate",
                table: "Meetings",
                column: "IsPrivate");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_IsRemoved",
                table: "Meetings",
                column: "IsRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_Latitude",
                table: "Meetings",
                column: "Latitude");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_Longitude",
                table: "Meetings",
                column: "Longitude");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_Size",
                table: "Meetings",
                column: "Size");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Action",
                table: "Messages",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AttachmentId",
                table: "Messages",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_Date",
                table: "Messages",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GroupId",
                table: "Messages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePhotos_AttachmentId",
                table: "ProfilePhotos",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePhotos_ProfileId",
                table: "ProfilePhotos",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Birthday",
                table: "Profiles",
                column: "Birthday");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Gender",
                table: "Profiles",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relations_IsRemoved",
                table: "Relations",
                column: "IsRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_Relations_RelatedUserId",
                table: "Relations",
                column: "RelatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Relations_UserId",
                table: "Relations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FacebookId",
                table: "Users",
                column: "FacebookId",
                unique: true,
                filter: "[FacebookId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GoogleId",
                table: "Users",
                column: "GoogleId",
                unique: true,
                filter: "[GoogleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsRemoved",
                table: "Users",
                column: "IsRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendInvitations");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "MeetingInvitations");

            migrationBuilder.DropTable(
                name: "MeetingRequestActivities");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ProfilePhotos");

            migrationBuilder.DropTable(
                name: "Relations");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "MeetingRequests");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
