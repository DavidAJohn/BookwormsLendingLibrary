using Microsoft.EntityFrameworkCore.Migrations;

namespace BookwormsAPI.Data.Migrations
{
    public partial class BorrowingBookIdFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestedBookId",
                table: "Requests",
                newName: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_BookId",
                table: "Requests",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Books_BookId",
                table: "Requests",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Books_BookId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_BookId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Requests",
                newName: "RequestedBookId");
        }
    }
}
