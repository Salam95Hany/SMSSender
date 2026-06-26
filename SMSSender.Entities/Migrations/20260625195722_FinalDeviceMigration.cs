using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class FinalDeviceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "device",
                table: "DeviceSyncVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "device",
                table: "DeviceRefreshInboxes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "device",
                table: "DeviceRefreshInboxes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "device",
                table: "DeviceHealthes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "device",
                table: "DeviceHealthes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                schema: "device",
                table: "DeviceHealthes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                schema: "device",
                table: "DeviceChangeLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "device",
                table: "DeviceChangeLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "device",
                table: "DeviceSyncVersions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "device",
                table: "DeviceRefreshInboxes");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "device",
                table: "DeviceRefreshInboxes");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "device",
                table: "DeviceHealthes");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "device",
                table: "DeviceHealthes");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                schema: "device",
                table: "DeviceHealthes");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "device",
                table: "DeviceChangeLogs");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                schema: "device",
                table: "DeviceChangeLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
