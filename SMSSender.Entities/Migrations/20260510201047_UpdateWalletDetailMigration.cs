using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWalletDetailMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthlyWithdrawalLimit",
                schema: "sms",
                table: "WalletDetails",
                newName: "UsedMonthlyWithdrawal");

            migrationBuilder.RenameColumn(
                name: "MonthlyDepositLimit",
                schema: "sms",
                table: "WalletDetails",
                newName: "UsedMonthlyDeposit");

            migrationBuilder.RenameColumn(
                name: "DailyWithdrawalLimit",
                schema: "sms",
                table: "WalletDetails",
                newName: "UsedDailyWithdrawal");

            migrationBuilder.RenameColumn(
                name: "DailyDepositLimit",
                schema: "sms",
                table: "WalletDetails",
                newName: "UsedDailyDeposit");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDailyResetDate",
                schema: "sms",
                table: "WalletDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMonthlyResetDate",
                schema: "sms",
                table: "WalletDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDailyResetDate",
                schema: "sms",
                table: "WalletDetails");

            migrationBuilder.DropColumn(
                name: "LastMonthlyResetDate",
                schema: "sms",
                table: "WalletDetails");

            migrationBuilder.RenameColumn(
                name: "UsedMonthlyWithdrawal",
                schema: "sms",
                table: "WalletDetails",
                newName: "MonthlyWithdrawalLimit");

            migrationBuilder.RenameColumn(
                name: "UsedMonthlyDeposit",
                schema: "sms",
                table: "WalletDetails",
                newName: "MonthlyDepositLimit");

            migrationBuilder.RenameColumn(
                name: "UsedDailyWithdrawal",
                schema: "sms",
                table: "WalletDetails",
                newName: "DailyWithdrawalLimit");

            migrationBuilder.RenameColumn(
                name: "UsedDailyDeposit",
                schema: "sms",
                table: "WalletDetails",
                newName: "DailyDepositLimit");
        }
    }
}
