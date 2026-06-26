using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSSender.Entities.Migrations
{
    /// <inheritdoc />
    public partial class LastMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivedStamp",
                schema: "sms",
                table: "SmsMessageLogs");

            migrationBuilder.DropColumn(
                name: "OperationMsgDateTime",
                schema: "sms",
                table: "MessageTransactions");

            migrationBuilder.DropColumn(
                name: "OperationSentDateTime",
                schema: "sms",
                table: "MessageTransactions");

            migrationBuilder.RenameColumn(
                name: "SentStamp",
                schema: "sms",
                table: "SmsMessageLogs",
                newName: "SmsGateId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "sms",
                table: "SmsMessageLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "sms",
                table: "MessageTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SmsGateId",
                schema: "sms",
                table: "MessageTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MessageTransactions_BranchId",
                schema: "sms",
                table: "MessageTransactions",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageTransactions_Branches_BranchId",
                schema: "sms",
                table: "MessageTransactions",
                column: "BranchId",
                principalSchema: "config",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageTransactions_Branches_BranchId",
                schema: "sms",
                table: "MessageTransactions");

            migrationBuilder.DropIndex(
                name: "IX_MessageTransactions_BranchId",
                schema: "sms",
                table: "MessageTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "sms",
                table: "SmsMessageLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "sms",
                table: "MessageTransactions");

            migrationBuilder.DropColumn(
                name: "SmsGateId",
                schema: "sms",
                table: "MessageTransactions");

            migrationBuilder.RenameColumn(
                name: "SmsGateId",
                schema: "sms",
                table: "SmsMessageLogs",
                newName: "SentStamp");

            migrationBuilder.AddColumn<string>(
                name: "ReceivedStamp",
                schema: "sms",
                table: "SmsMessageLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OperationMsgDateTime",
                schema: "sms",
                table: "MessageTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OperationSentDateTime",
                schema: "sms",
                table: "MessageTransactions",
                type: "datetime2",
                nullable: true);
        }
    }
}
