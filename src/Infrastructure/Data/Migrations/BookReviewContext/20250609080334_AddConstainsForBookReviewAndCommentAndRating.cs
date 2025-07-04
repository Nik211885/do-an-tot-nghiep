using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.BookReviewContext
{
    /// <inheritdoc />
    public partial class AddConstainsForBookReviewAndCommentAndRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ratings_BookReviewId",
                schema: "BookReview",
                table: "Ratings",
                column: "BookReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BookReviewId",
                schema: "BookReview",
                table: "Comments",
                column: "BookReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_BookReviews_BookReviewId",
                schema: "BookReview",
                table: "Comments",
                column: "BookReviewId",
                principalSchema: "BookReview",
                principalTable: "BookReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_BookReviews_BookReviewId",
                schema: "BookReview",
                table: "Ratings",
                column: "BookReviewId",
                principalSchema: "BookReview",
                principalTable: "BookReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_BookReviews_BookReviewId",
                schema: "BookReview",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_BookReviews_BookReviewId",
                schema: "BookReview",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_BookReviewId",
                schema: "BookReview",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Comments_BookReviewId",
                schema: "BookReview",
                table: "Comments");
        }
    }
}
