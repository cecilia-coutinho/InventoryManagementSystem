using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventorySystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationshipProductAndInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Products_ProductsProductId",
                table: "Inventory");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_Orders_FkCustomerID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_ProductsProductId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "ProductsProductId",
                table: "Inventory");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_Orders_FkCustomerID",
                table: "Orders",
                sql: "(OrderType = 'Sale' AND FkCustomerID IS NOT NULL AND FkSupplierID IS NULL) OR\r\n              (OrderType = 'Purchase' AND FkSupplierID IS NOT NULL AND FkCustomerID IS NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_FkProductId",
                table: "Inventory",
                column: "FkProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Products_FkProductId",
                table: "Inventory",
                column: "FkProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Products_FkProductId",
                table: "Inventory");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_Orders_FkCustomerID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_FkProductId",
                table: "Inventory");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsProductId",
                table: "Inventory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CHK_Orders_FkCustomerID",
                table: "Orders",
                sql: "(OrderType = 'Sale' AND FkCustomerID IS NOT NULL AND FkSupplierID IS NULL) OR\r\n                  (OrderType = 'Purchase' AND FkSupplierID IS NOT NULL AND FkCustomerID IS NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductsProductId",
                table: "Inventory",
                column: "ProductsProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Products_ProductsProductId",
                table: "Inventory",
                column: "ProductsProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
