using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAnimeWorld.App.Data.Migrations
{
    public partial class FixedAnimeLinkNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeLinks_AnimeSourceLinks_SourceNameId",
                table: "AnimeLinks");

            migrationBuilder.RenameColumn(
                name: "SourceNameId",
                table: "AnimeLinks",
                newName: "SourceId");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "AnimeLinks",
                newName: "SourceName");

            migrationBuilder.RenameIndex(
                name: "IX_AnimeLinks_SourceNameId",
                table: "AnimeLinks",
                newName: "IX_AnimeLinks_SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeLinks_AnimeSourceLinks_SourceId",
                table: "AnimeLinks",
                column: "SourceId",
                principalTable: "AnimeSourceLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimeLinks_AnimeSourceLinks_SourceId",
                table: "AnimeLinks");

            migrationBuilder.RenameColumn(
                name: "SourceName",
                table: "AnimeLinks",
                newName: "Source");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "AnimeLinks",
                newName: "SourceNameId");

            migrationBuilder.RenameIndex(
                name: "IX_AnimeLinks_SourceId",
                table: "AnimeLinks",
                newName: "IX_AnimeLinks_SourceNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimeLinks_AnimeSourceLinks_SourceNameId",
                table: "AnimeLinks",
                column: "SourceNameId",
                principalTable: "AnimeSourceLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
