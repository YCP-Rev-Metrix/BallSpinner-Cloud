using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCore.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hand",
                schema: "combinedDB",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastLogin",
                schema: "combinedDB",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Establishments",
                schema: "combinedDB",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lanes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establishments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                schema: "combinedDB",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lanes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Win = table.Column<int>(type: "int", nullable: false),
                    StartingLane = table.Column<int>(type: "int", nullable: false),
                    SessionID = table.Column<int>(type: "int", nullable: false),
                    TeamResult = table.Column<int>(type: "int", nullable: false),
                    IndividualResult = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                schema: "combinedDB",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    EstablishmentID = table.Column<int>(type: "int", nullable: false),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<int>(type: "int", nullable: false),
                    TeamOpponent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndividualOpponent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Stats = table.Column<int>(type: "int", nullable: false),
                    TeamRecord = table.Column<int>(type: "int", nullable: false),
                    IndividualRecord = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Shots",
                schema: "combinedDB",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SmartDotID = table.Column<int>(type: "int", nullable: false),
                    SesssionID = table.Column<int>(type: "int", nullable: false),
                    BallID = table.Column<int>(type: "int", nullable: false),
                    FrameID = table.Column<int>(type: "int", nullable: false),
                    ShotNumber = table.Column<int>(type: "int", nullable: false),
                    LeaveType = table.Column<int>(type: "int", nullable: false),
                    Side = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shots", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Establishments",
                schema: "combinedDB");

            migrationBuilder.DropTable(
                name: "Games",
                schema: "combinedDB");

            migrationBuilder.DropTable(
                name: "Sessions",
                schema: "combinedDB");

            migrationBuilder.DropTable(
                name: "Shots",
                schema: "combinedDB");

            migrationBuilder.DropColumn(
                name: "Hand",
                schema: "combinedDB",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                schema: "combinedDB",
                table: "Users");
        }
    }
}
