using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class ChangedPollKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoSongName_Room_RoomId",
                table: "InfoSongName");

            migrationBuilder.DropForeignKey(
                name: "FK_Poll_Room_RoomId",
                table: "Poll");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropIndex(
                name: "IX_Clients_Username",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Poll",
                table: "Poll");

            migrationBuilder.DropIndex(
                name: "IX_Poll_RoomId",
                table: "Poll");

            migrationBuilder.DropColumn(
                name: "PollId",
                table: "Poll");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Poll");

            migrationBuilder.RenameTable(
                name: "Poll",
                newName: "Polls");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "InfoSongName",
                newName: "JukeboxRoomRoomId");

            migrationBuilder.RenameIndex(
                name: "IX_InfoSongName_RoomId",
                table: "InfoSongName",
                newName: "IX_InfoSongName_JukeboxRoomRoomId");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SongName",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "JukeboxRoomRoomId",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Polls",
                table: "Polls",
                column: "SongName");

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hostname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PinCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnlineUsers = table.Column<int>(type: "int", nullable: false),
                    OpeningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SongName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsPlaying = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polls_JukeboxRoomRoomId",
                table: "Polls",
                column: "JukeboxRoomRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoSongName_Rooms_JukeboxRoomRoomId",
                table: "InfoSongName",
                column: "JukeboxRoomRoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Rooms_JukeboxRoomRoomId",
                table: "Polls",
                column: "JukeboxRoomRoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InfoSongName_Rooms_JukeboxRoomRoomId",
                table: "InfoSongName");

            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Rooms_JukeboxRoomRoomId",
                table: "Polls");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Polls",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_JukeboxRoomRoomId",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "JukeboxRoomRoomId",
                table: "Polls");

            migrationBuilder.RenameTable(
                name: "Polls",
                newName: "Poll");

            migrationBuilder.RenameColumn(
                name: "JukeboxRoomRoomId",
                table: "InfoSongName",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_InfoSongName_JukeboxRoomRoomId",
                table: "InfoSongName",
                newName: "IX_InfoSongName_RoomId");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SongName",
                table: "Poll",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "PollId",
                table: "Poll",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                table: "Poll",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Poll",
                table: "Poll",
                column: "PollId");

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Hostname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPlaying = table.Column<bool>(type: "bit", nullable: false),
                    OnlineUsers = table.Column<int>(type: "int", nullable: false),
                    OpeningTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PinCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<TimeSpan>(type: "time", nullable: false),
                    SongName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Username",
                table: "Clients",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Poll_RoomId",
                table: "Poll",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_InfoSongName_Room_RoomId",
                table: "InfoSongName",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Poll_Room_RoomId",
                table: "Poll",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
