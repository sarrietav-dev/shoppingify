using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shoppingify.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCheckedcolumntothelineitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "LineItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "LineItems");
        }
    }
}
