using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class ProfitClosingsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfitClosings",
                schema: "sms",
                columns: table => new
                {
                    ProfitClosingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalProfit = table.Column<double>(type: "float", nullable: false),
                    NetProfit = table.Column<double>(type: "float", nullable: false),
                    CashBalanceBefore = table.Column<double>(type: "float", nullable: false),
                    CashBalanceAfter = table.Column<double>(type: "float", nullable: false),
                    ClosedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfitClosings", x => x.ProfitClosingId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfitClosings",
                schema: "sms");
        }
    }
}
