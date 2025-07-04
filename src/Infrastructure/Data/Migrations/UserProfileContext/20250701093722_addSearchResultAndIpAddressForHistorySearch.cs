using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.UserProfileContext
{
    /// <inheritdoc />
    public partial class addSearchResultAndIpAddressForHistorySearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                schema: "UserProfile",
                table: "SearchHistory",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SearchCout",
                schema: "UserProfile",
                table: "SearchHistory",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                schema: "UserProfile",
                table: "SearchHistory");

            migrationBuilder.DropColumn(
                name: "SearchCout",
                schema: "UserProfile",
                table: "SearchHistory");
        }
    }
}
