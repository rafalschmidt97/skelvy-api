using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skelvy.Persistence.Migrations
{
    public partial class RenameUserProfileToProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfilePhotos");

            migrationBuilder.DropTable(
                name: "UserProfiles");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilePhotos");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Birthday = table.Column<DateTimeOffset>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Gender = table.Column<string>(maxLength: 15, nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    UserId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfilePhotos",
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
                    table.PrimaryKey("PK_UserProfilePhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfilePhotos_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfilePhotos_UserProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfilePhotos_AttachmentId",
                table: "UserProfilePhotos",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfilePhotos_ProfileId",
                table: "UserProfilePhotos",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_Birthday",
                table: "UserProfiles",
                column: "Birthday");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_Gender",
                table: "UserProfiles",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);
        }
    }
}
