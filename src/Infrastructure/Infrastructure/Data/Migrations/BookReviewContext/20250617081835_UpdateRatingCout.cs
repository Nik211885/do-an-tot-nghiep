using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Data.Migrations.BookReviewContext
{
    /// <inheritdoc />
    public partial class UpdateRatingCout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                schema: "BookReview",
                table: "BookReviews");

            migrationBuilder.AddColumn<long>(
                name: "TotalRating",
                schema: "BookReview",
                table: "BookReviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalRating",
                schema: "BookReview",
                table: "BookReviews");

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                schema: "BookReview",
                table: "BookReviews",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
