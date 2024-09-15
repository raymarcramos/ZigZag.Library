using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZigZag.Library.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Isbn = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    PublishedDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
