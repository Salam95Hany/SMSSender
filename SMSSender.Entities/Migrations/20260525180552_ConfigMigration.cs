using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class ConfigMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "config");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "sms",
                table: "WalletDetails",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "sms",
                table: "WalletDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("4E4F1CDF-192C-4DB9-B16D-CB633A874FF4"));

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "sms",
                table: "SmsMessageLogs",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "sms",
                table: "SmsMessageLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("4E4F1CDF-192C-4DB9-B16D-CB633A874FF4"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ClosedDate",
                schema: "sms",
                table: "ProfitClosings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "CashBalanceBefore",
                schema: "sms",
                table: "ProfitClosings",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "CashBalanceAfter",
                schema: "sms",
                table: "ProfitClosings",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "sms",
                table: "ProfitClosings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "sms",
                table: "ProfitClosings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("4E4F1CDF-192C-4DB9-B16D-CB633A874FF4"));

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "sms",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "sms",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("4E4F1CDF-192C-4DB9-B16D-CB633A874FF4"));

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "sms",
                table: "MessageTransactions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "sms",
                table: "MessageTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("4E4F1CDF-192C-4DB9-B16D-CB633A874FF4"));

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "sms",
                table: "CashBoxes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                schema: "sms",
                table: "CashBoxes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("4E4F1CDF-192C-4DB9-B16D-CB633A874FF4"));

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("4E4F1CDF-192C-4DB9-B16D-CB633A874FF4"));

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "config",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSubscriptions",
                schema: "config",
                columns: table => new
                {
                    CustomerSubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSubscriptions", x => x.CustomerSubscriptionId);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                schema: "config",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    MaxUsers = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.PlanId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers",
                schema: "config");

            migrationBuilder.DropTable(
                name: "CustomerSubscriptions",
                schema: "config");

            migrationBuilder.DropTable(
                name: "Plans",
                schema: "config");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "sms",
                table: "WalletDetails");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "sms",
                table: "WalletDetails");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "sms",
                table: "SmsMessageLogs");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "sms",
                table: "SmsMessageLogs");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "sms",
                table: "ProfitClosings");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "sms",
                table: "ProfitClosings");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "sms",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "sms",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "sms",
                table: "MessageTransactions");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "sms",
                table: "MessageTransactions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "sms",
                table: "CashBoxes");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "sms",
                table: "CashBoxes");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ClosedDate",
                schema: "sms",
                table: "ProfitClosings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CashBalanceBefore",
                schema: "sms",
                table: "ProfitClosings",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CashBalanceAfter",
                schema: "sms",
                table: "ProfitClosings",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
