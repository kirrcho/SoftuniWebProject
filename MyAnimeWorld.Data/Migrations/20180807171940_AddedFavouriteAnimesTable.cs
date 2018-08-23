using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAnimeWorld.App.Data.Migrations
{
    public partial class AddedFavouriteAnimesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRatedAnimes",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    AnimeId = table.Column<int>(nullable: false),
                    IsFavourite = table.Column<bool>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatedAnimes", x => new { x.AnimeId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRatedAnimes_AnimeSeries_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "AnimeSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRatedAnimes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRatedAnimes_UserId",
                table: "UserRatedAnimes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRatedAnimes");
        }
    }
}
