using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceProcessor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceUploadedByFkAndNullableFilePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Vendors_VendorId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "SubTotal",
                table: "Invoices",
                newName: "Subtotal");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Invoices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UploadedById",
                table: "Invoices",
                column: "UploadedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Users_UploadedById",
                table: "Invoices",
                column: "UploadedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Vendors_VendorId",
                table: "Invoices",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Users_UploadedById",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Vendors_VendorId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_UploadedById",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "Subtotal",
                table: "Invoices",
                newName: "SubTotal");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Invoices",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Vendors_VendorId",
                table: "Invoices",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
