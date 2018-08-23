using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAnimeWorld.App.Data.Migrations
{
    public partial class AddedMultipleCategoriesForSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeSeries_Categories_CategoryId",
                table: "AnimeSeries");

            migrationBuilder.DropIndex(
                name: "IX_AnimeSeries_CategoryId",
                table: "AnimeSeries");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AnimeSeries");

            migrationBuilder.CreateTable(
                name: "AnimeSeriesCategories",
                columns: table => new
                {
                    AnimeId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeSeriesCategories", x => new { x.AnimeId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_AnimeSeriesCategories_AnimeSeries_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "AnimeSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeSeriesCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeriesCategories_CategoryId",
                table: "AnimeSeriesCategories",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeSeriesCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "AnimeSeries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeries_CategoryId",
                table: "AnimeSeries",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeSeries_Categories_CategoryId",
                table: "AnimeSeries",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
