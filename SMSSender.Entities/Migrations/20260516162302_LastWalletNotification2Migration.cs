using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class LastWalletNotification2Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastMonthlyLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails",
                newName: "LastMonthlyWithdrawalLimitNotificationDate");

            migrationBuilder.RenameColumn(
                name: "LastDailyLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails",
                newName: "LastMonthlyDepositLimitNotificationDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastRechargeDate",
                schema: "sms",
                table: "WalletDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDailyDepositLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDailyWithdrawalLimitNotificationDate",
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
                name: "LastDailyDepositLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails");

            migrationBuilder.DropColumn(
                name: "LastDailyWithdrawalLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails");

            migrationBuilder.RenameColumn(
                name: "LastMonthlyWithdrawalLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails",
                newName: "LastMonthlyLimitNotificationDate");

            migrationBuilder.RenameColumn(
                name: "LastMonthlyDepositLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails",
                newName: "LastDailyLimitNotificationDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastRechargeDate",
                schema: "sms",
                table: "WalletDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
