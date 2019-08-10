using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skelvy.Persistence.Migrations
{
    public partial class RenameDrinkTypeWithActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_DrinkTypes_DrinkTypeId",
                table: "Meetings");

            migrationBuilder.DropTable(
                name: "MeetingRequestDrinkTypes");

            migrationBuilder.DropTable(
                name: "DrinkTypes");

            migrationBuilder.RenameColumn(
                name: "DrinkTypeId",
                table: "Meetings",
                newName: "ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_DrinkTypeId",
                table: "Meetings",
                newName: "IX_Meetings_ActivityId");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Name",
                table: "Activities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequestActivities_ActivityId",
                table: "MeetingRequestActivities",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Activities_ActivityId",
                table: "Meetings",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Activities_ActivityId",
                table: "Meetings");

            migrationBuilder.DropTable(
                name: "MeetingRequestActivities");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "Meetings",
                newName: "DrinkTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_ActivityId",
                table: "Meetings",
                newName: "IX_Meetings_DrinkTypeId");

            migrationBuilder.CreateTable(
                name: "DrinkTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingRequestDrinkTypes",
                columns: table => new
                {
                    MeetingRequestId = table.Column<int>(nullable: false),
                    DrinkTypeId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRequestDrinkTypes", x => new { x.MeetingRequestId, x.DrinkTypeId });
                    table.ForeignKey(
                        name: "FK_MeetingRequestDrinkTypes_DrinkTypes_DrinkTypeId",
                        column: x => x.DrinkTypeId,
                        principalTable: "DrinkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MeetingRequestDrinkTypes_MeetingRequests_MeetingRequestId",
                        column: x => x.MeetingRequestId,
                        principalTable: "MeetingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrinkTypes_Name",
                table: "DrinkTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeetingRequestDrinkTypes_DrinkTypeId",
                table: "MeetingRequestDrinkTypes",
                column: "DrinkTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_DrinkTypes_DrinkTypeId",
                table: "Meetings",
                column: "DrinkTypeId",
                principalTable: "DrinkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
