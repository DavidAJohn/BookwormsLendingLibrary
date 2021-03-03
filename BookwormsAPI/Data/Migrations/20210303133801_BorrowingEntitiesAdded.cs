using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookwormsAPI.Data.Migrations
{
    public partial class BorrowingEntitiesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Copies",
                table: "Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BorrowerEmail = table.Column<string>(type: "TEXT", nullable: false),
                    SendToAddress_FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    SendToAddress_LastName = table.Column<string>(type: "TEXT", nullable: true),
                    SendToAddress_Street = table.Column<string>(type: "TEXT", nullable: true),
                    SendToAddress_City = table.Column<string>(type: "TEXT", nullable: true),
                    SendToAddress_County = table.Column<string>(type: "TEXT", nullable: true),
                    SendToAddress_PostCode = table.Column<string>(type: "TEXT", nullable: true),
                    RequestedBookId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    DateRequested = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateSent = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateDue = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateReturned = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropColumn(
                name: "Copies",
                table: "Books");
        }
    }
}
