using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JukeboxClient",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SessionKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JukeboxClient", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "JukeboxHost",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JukeboxHost", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "JukeboxSession",
                columns: table => new
                {
                    SessionKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HostName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActiveUsers = table.Column<int>(type: "int", nullable: false),
                    TotalUsers = table.Column<int>(type: "int", nullable: false),
                    SongName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SongDuration = table.Column<double>(type: "float", nullable: false),
                    SongPosition = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JukeboxSession", x => x.SessionKey);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JukeboxClient");

            migrationBuilder.DropTable(
                name: "JukeboxHost");

            migrationBuilder.DropTable(
                name: "JukeboxSession");
        }
    }
}
