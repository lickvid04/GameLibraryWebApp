using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_User_User_ID",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_User_ID",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "User_ID",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "GamesUser",
                columns: table => new
                {
                    GamesListGame_ID = table.Column<int>(type: "integer", nullable: false),
                    UsersUser_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesUser", x => new { x.GamesListGame_ID, x.UsersUser_ID });
                    table.ForeignKey(
                        name: "FK_GamesUser_Games_GamesListGame_ID",
                        column: x => x.GamesListGame_ID,
                        principalTable: "Games",
                        principalColumn: "Game_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamesUser_User_UsersUser_ID",
                        column: x => x.UsersUser_ID,
                        principalTable: "User",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamesUser_UsersUser_ID",
                table: "GamesUser",
                column: "UsersUser_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamesUser");

            migrationBuilder.AddColumn<int>(
                name: "User_ID",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_User_ID",
                table: "Games",
                column: "User_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_User_User_ID",
                table: "Games",
                column: "User_ID",
                principalTable: "User",
                principalColumn: "User_ID");
        }
    }
}
