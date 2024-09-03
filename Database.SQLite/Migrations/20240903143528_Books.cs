using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class Books : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Course",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "SharedCode",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "BookCategories");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "BookCategories");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "BookCategories");

            migrationBuilder.AddColumn<int>(
                name: "BookEntityBookId",
                table: "Words",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Words",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookName = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    AuditYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Grade = table.Column<string>(type: "TEXT", nullable: false),
                    Semester = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                });

            migrationBuilder.CreateTable(
                name: "BookCategoryMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategoryMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookCategoryMappings_BookCategories_BookCategoryId",
                        column: x => x.BookCategoryId,
                        principalTable: "BookCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCategoryMappings_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_BookEntityBookId",
                table: "Words",
                column: "BookEntityBookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCategoryMappings_BookCategoryId",
                table: "BookCategoryMappings",
                column: "BookCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCategoryMappings_BookId",
                table: "BookCategoryMappings",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Books_BookEntityBookId",
                table: "Words",
                column: "BookEntityBookId",
                principalTable: "Books",
                principalColumn: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_Books_BookEntityBookId",
                table: "Words");

            migrationBuilder.DropTable(
                name: "BookCategoryMappings");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Words_BookEntityBookId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "BookEntityBookId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Words");

            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "Words",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SharedCode",
                table: "Words",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "BookCategories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "BookCategories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "BookCategories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
