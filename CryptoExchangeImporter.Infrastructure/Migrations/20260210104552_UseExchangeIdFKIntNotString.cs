using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoExchangeImporter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseExchangeIdFKIntNotString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
