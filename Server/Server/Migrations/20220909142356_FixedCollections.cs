using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class FixedCollections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Rooms_JukeboxRoomRoomId",
                table: "Polls");

            migrationBuilder.DropTable(
                name: "InfoHostname");

            migrationBuilder.DropTable(
                name: "InfoSongName");

            migrationBuilder.DropIndex(
                name: "IX_Polls_JukeboxRoomRoomId",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "JukeboxRoomRoomId",
                table: "Polls");

            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                table: "Polls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Polls");

            migrationBuilder.AddColumn<string>(
                name: "JukeboxRoomRoomId",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InfoHostname",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hostname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JukeboxClientToken = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoHostname", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoHostname_Clients_JukeboxClientToken",
                        column: x => x.JukeboxClientToken,
                        principalTable: "Clients",
                        principalColumn: "Token");
                });

            migrationBuilder.CreateTable(
                name: "InfoSongName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JukeboxClientToken = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    JukeboxRoomRoomId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SongName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoSongName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoSongName_Clients_JukeboxClientToken",
                        column: x => x.JukeboxClientToken,
                        principalTable: "Clients",
                        principalColumn: "Token");
                    table.ForeignKey(
                        name: "FK_InfoSongName_Rooms_JukeboxRoomRoomId",
                        column: x => x.JukeboxRoomRoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polls_JukeboxRoomRoomId",
                table: "Polls",
                column: "JukeboxRoomRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoHostname_JukeboxClientToken",
                table: "InfoHostname",
                column: "JukeboxClientToken");

            migrationBuilder.CreateIndex(
                name: "IX_InfoSongName_JukeboxClientToken",
                table: "InfoSongName",
                column: "JukeboxClientToken");

            migrationBuilder.CreateIndex(
                name: "IX_InfoSongName_JukeboxRoomRoomId",
                table: "InfoSongName",
                column: "JukeboxRoomRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Rooms_JukeboxRoomRoomId",
                table: "Polls",
                column: "JukeboxRoomRoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");
        }
    }
}
