using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAnimeWorld.App.Data.Migrations
{
    public partial class AddedBasicTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimeSourceLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeSourceLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnimeSeries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeSeries_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnimeSeriesId = table.Column<int>(nullable: false),
                    EpisodeNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_AnimeSeries_AnimeSeriesId",
                        column: x => x.AnimeSeriesId,
                        principalTable: "AnimeSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnimeId = table.Column<int>(nullable: false),
                    SourceNameId = table.Column<int>(nullable: false),
                    Source = table.Column<string>(nullable: false),
                    AnimeEpisodeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeLinks_Episodes_AnimeEpisodeId",
                        column: x => x.AnimeEpisodeId,
                        principalTable: "Episodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimeLinks_AnimeSeries_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "AnimeSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeLinks_AnimeSourceLinks_SourceNameId",
                        column: x => x.SourceNameId,
                        principalTable: "AnimeSourceLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeLinks_AnimeEpisodeId",
                table: "AnimeLinks",
                column: "AnimeEpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeLinks_AnimeId",
                table: "AnimeLinks",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeLinks_SourceNameId",
                table: "AnimeLinks",
                column: "SourceNameId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeries_CategoryId",
                table: "AnimeSeries",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeries_Title",
                table: "AnimeSeries",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_AnimeSeriesId",
                table: "Episodes",
                column: "AnimeSeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeLinks");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "AnimeSourceLinks");

            migrationBuilder.DropTable(
                name: "AnimeSeries");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
