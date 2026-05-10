using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class WalletDetailMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WalletDetails",
                schema: "sms",
                columns: table => new
                {
                    WalletDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastRechargeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    MonthlyDepositLimit = table.Column<double>(type: "float", nullable: false),
                    MonthlyWithdrawalLimit = table.Column<double>(type: "float", nullable: false),
                    DailyDepositLimit = table.Column<double>(type: "float", nullable: false),
                    DailyWithdrawalLimit = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletDetails", x => x.WalletDetailId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletDetails",
                schema: "sms");
        }
    }
}
