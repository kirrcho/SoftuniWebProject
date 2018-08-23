using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAnimeWorld.App.Data.Migrations
{
    public partial class FixedForgottenEpisodeTableInLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeLinks_Episodes_AnimeEpisodeId",
                table: "AnimeLinks");

            migrationBuilder.DropIndex(
                name: "IX_AnimeLinks_AnimeEpisodeId",
                table: "AnimeLinks");

            migrationBuilder.DropColumn(
                name: "AnimeEpisodeId",
                table: "AnimeLinks");

            migrationBuilder.AddColumn<int>(
                name: "EpisodeId",
                table: "AnimeLinks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeLinks_EpisodeId",
                table: "AnimeLinks",
                column: "EpisodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeLinks_Episodes_EpisodeId",
                table: "AnimeLinks",
                column: "EpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeLinks_Episodes_EpisodeId",
                table: "AnimeLinks");

            migrationBuilder.DropIndex(
                name: "IX_AnimeLinks_EpisodeId",
                table: "AnimeLinks");

            migrationBuilder.DropColumn(
                name: "EpisodeId",
                table: "AnimeLinks");

            migrationBuilder.AddColumn<int>(
                name: "AnimeEpisodeId",
                table: "AnimeLinks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeLinks_AnimeEpisodeId",
                table: "AnimeLinks",
                column: "AnimeEpisodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeLinks_Episodes_AnimeEpisodeId",
                table: "AnimeLinks",
                column: "AnimeEpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
