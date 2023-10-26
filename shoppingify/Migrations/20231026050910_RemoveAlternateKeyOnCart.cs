using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shoppingify.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAlternateKeyOnCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Carts_CartOwnerId",
                table: "Carts");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CartOwnerId",
                table: "Carts",
                column: "CartOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_CartOwners_CartOwnerId",
                table: "Carts",
                column: "CartOwnerId",
                principalTable: "CartOwners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_CartOwners_CartOwnerId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CartOwnerId",
                table: "Carts");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Carts_CartOwnerId",
                table: "Carts",
                column: "CartOwnerId");
        }
    }
}
