using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class DevicesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "device");

            migrationBuilder.CreateTable(
                name: "DeviceChangeLogs",
                schema: "device",
                columns: table => new
                {
                    DeviceChangeLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceChangeLogs", x => x.DeviceChangeLogId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceHealthes",
                schema: "device",
                columns: table => new
                {
                    DeviceHealthId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessagesFailed = table.Column<int>(type: "int", nullable: false),
                    ConnectionStatus = table.Column<int>(type: "int", nullable: false),
                    ConnectionTransport = table.Column<int>(type: "int", nullable: false),
                    BatteryLevel = table.Column<int>(type: "int", nullable: false),
                    BatteryCharging = table.Column<bool>(type: "bit", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceHealthes", x => x.DeviceHealthId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceRefreshInboxes",
                schema: "device",
                columns: table => new
                {
                    DeviceRefreshInboxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshStatus = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceRefreshInboxes", x => x.DeviceRefreshInboxId);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "device",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceUniqueId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sim1Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sim2Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sim1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sim2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                });

            migrationBuilder.CreateTable(
                name: "DeviceSyncVersions",
                schema: "device",
                columns: table => new
                {
                    DeviceSyncVersionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentVersion = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceSyncVersions", x => x.DeviceSyncVersionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceChangeLogs",
                schema: "device");

            migrationBuilder.DropTable(
                name: "DeviceHealthes",
                schema: "device");

            migrationBuilder.DropTable(
                name: "DeviceRefreshInboxes",
                schema: "device");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "device");

            migrationBuilder.DropTable(
                name: "DeviceSyncVersions",
                schema: "device");
        }
    }
}
