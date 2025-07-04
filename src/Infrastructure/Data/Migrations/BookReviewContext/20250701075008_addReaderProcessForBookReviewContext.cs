using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.BookReviewContext
{
    /// <inheritdoc />
    public partial class addReaderProcessForBookReviewContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reader",
                schema: "BookReview",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookReviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReaderAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reader_BookReviews_BookReviewId",
                        column: x => x.BookReviewId,
                        principalSchema: "BookReview",
                        principalTable: "BookReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reader_BookReviewId",
                schema: "BookReview",
                table: "Reader",
                column: "BookReviewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reader",
                schema: "BookReview");
        }
    }
}
