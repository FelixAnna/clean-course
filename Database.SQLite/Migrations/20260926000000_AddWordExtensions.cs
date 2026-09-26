using Database.SQLite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.SQLite.Migrations;

[DbContext(typeof(SQLiteCourseContext))]
[Migration("20260926000000_AddWordExtensions")]
public partial class AddWordExtensions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "WordExtensions",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                WordId = table.Column<int>(type: "INTEGER", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                Content = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WordExtensions", x => x.Id);
                table.ForeignKey(
                    name: "FK_WordExtensions_Words_WordId",
                    column: x => x.WordId,
                    principalTable: "Words",
                    principalColumn: "WordId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_WordExtensions_WordId",
            table: "WordExtensions",
            column: "WordId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "WordExtensions");
    }
}