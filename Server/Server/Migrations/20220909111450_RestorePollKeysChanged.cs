using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class RestorePollKeysChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Polls",
                table: "Polls");

            migrationBuilder.RenameColumn(
                name: "Place",
                table: "Polls",
                newName: "PollId");

            migrationBuilder.AlterColumn<string>(
                name: "SongName",
                table: "Polls",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Polls",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Polls",
                table: "Polls",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Polls",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Polls");

            migrationBuilder.RenameColumn(
                name: "PollId",
                table: "Polls",
                newName: "Place");

            migrationBuilder.AlterColumn<string>(
                name: "SongName",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Polls",
                table: "Polls",
                column: "SongName");
        }
    }
}
