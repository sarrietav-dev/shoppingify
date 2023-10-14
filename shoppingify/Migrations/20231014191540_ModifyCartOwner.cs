using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shoppingify.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCartOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_CartOwners_CartOwnerId",
                table: "Carts");

            migrationBuilder.AddColumn<Guid>(
                name: "ActiveCart",
                table: "CartOwners",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveCart",
                table: "CartOwners");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_CartOwners_CartOwnerId",
                table: "Carts",
                column: "CartOwnerId",
                principalTable: "CartOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
