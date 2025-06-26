using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations.OrderContext
{
    /// <inheritdoc />
    public partial class AddColumnOrderCodeForSinglePaymentForOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentUrl",
                schema: "OrderBook",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "OrderCode",
                schema: "OrderBook",
                table: "Orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderCode",
                schema: "OrderBook",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "PaymentUrl",
                schema: "OrderBook",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
