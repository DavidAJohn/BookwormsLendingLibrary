using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookwormsAPI.Data.Migrations
{
    public partial class AdditionsToBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "Books",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RequestCount",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RequestCount",
                table: "Books");
        }
    }
}
