using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class LastWalletNotificationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastDailyLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMonthlyLimitNotificationDate",
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
                name: "LastDailyLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails");

            migrationBuilder.DropColumn(
                name: "LastMonthlyLimitNotificationDate",
                schema: "sms",
                table: "WalletDetails");
        }
    }
}
