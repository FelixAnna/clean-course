using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class BookWord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_Books_BookEntityBookId",
                table: "Words");

            migrationBuilder.DropIndex(
                name: "IX_Words_BookEntityBookId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "BookEntityBookId",
                table: "Words");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Words",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Words_BookId",
                table: "Words",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Books_BookId",
                table: "Words",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_Books_BookId",
                table: "Words");

            migrationBuilder.DropIndex(
                name: "IX_Words_BookId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Words");

            migrationBuilder.AddColumn<int>(
                name: "BookEntityBookId",
                table: "Words",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Words_BookEntityBookId",
                table: "Words",
                column: "BookEntityBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Books_BookEntityBookId",
                table: "Words",
                column: "BookEntityBookId",
                principalTable: "Books",
                principalColumn: "BookId");
        }
    }
}
