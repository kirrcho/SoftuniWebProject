using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAnimeWorld.App.Data.Migrations
{
    public partial class AddedProfileDateCreationColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreatedAt",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreatedAt",
                table: "AspNetUsers");
        }
    }
}
