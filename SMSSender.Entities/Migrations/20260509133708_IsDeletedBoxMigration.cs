using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class IsDeletedBoxMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CashBoxNumber",
                schema: "sms",
                table: "CashBoxes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "sms",
                table: "CashBoxes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashBoxNumber",
                schema: "sms",
                table: "CashBoxes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "sms",
                table: "CashBoxes");
        }
    }
}
