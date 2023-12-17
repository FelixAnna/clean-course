using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckingHistories_Kids_KidId",
                table: "CheckingHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckingHistories_Words_WordId",
                table: "CheckingHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckingHistories_Kids_KidId",
                table: "CheckingHistories",
                column: "KidId",
                principalTable: "Kids",
                principalColumn: "KidId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckingHistories_Words_WordId",
                table: "CheckingHistories",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckingHistories_Kids_KidId",
                table: "CheckingHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckingHistories_Words_WordId",
                table: "CheckingHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckingHistories_Kids_KidId",
                table: "CheckingHistories",
                column: "KidId",
                principalTable: "Kids",
                principalColumn: "KidId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckingHistories_Words_WordId",
                table: "CheckingHistories",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "WordId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
