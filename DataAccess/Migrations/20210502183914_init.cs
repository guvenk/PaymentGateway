﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "merchant",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_merchant", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shopper",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    cardNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    expireMonth = table.Column<int>(type: "int", nullable: false),
                    expireYear = table.Column<int>(type: "int", nullable: false),
                    cvv = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopper", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(15,3)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    isSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    merchantId = table.Column<long>(type: "bigint", nullable: false),
                    shopperId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_merchant_merchantId",
                        column: x => x.merchantId,
                        principalTable: "merchant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payment_shopper_shopperId",
                        column: x => x.shopperId,
                        principalTable: "shopper",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "merchant",
                columns: new[] { "id", "name" },
                values: new object[] { 1L, "Amazon" });

            migrationBuilder.InsertData(
                table: "shopper",
                columns: new[] { "id", "cardNumber", "cvv", "expireMonth", "expireYear", "firstName", "lastName" },
                values: new object[] { 1L, "3333-4444-5555-6666", 333, 12, 2025, "John", "Smith" });

            migrationBuilder.InsertData(
                table: "payment",
                columns: new[] { "id", "amount", "createdDate", "currency", "isSuccessful", "merchantId", "shopperId" },
                values: new object[] { new Guid("0a036fba-2bbf-4530-a90c-c0d07c3fd23a"), 1000.000m, new DateTime(2021, 5, 2, 18, 39, 14, 367, DateTimeKind.Utc).AddTicks(2146), "EUR", true, 1L, 1L });

            migrationBuilder.CreateIndex(
                name: "IX_payment_merchantId",
                table: "payment",
                column: "merchantId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_shopperId",
                table: "payment",
                column: "shopperId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "merchant");

            migrationBuilder.DropTable(
                name: "shopper");
        }
    }
}
