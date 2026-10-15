using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceProcessor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceConfirmationAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                table: "Invoices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConfirmedById",
                table: "Invoices",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ConfirmedById",
                table: "Invoices",
                column: "ConfirmedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Users_ConfirmedById",
                table: "Invoices",
                column: "ConfirmedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Users_ConfirmedById",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ConfirmedById",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ConfirmedById",
                table: "Invoices");
        }
    }
}
