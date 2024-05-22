using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expenses.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWishList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_CategoryWishlists_CategoryId",
                table: "WishlistItems");

            migrationBuilder.DropIndex(
                name: "IX_WishlistItems_CategoryId",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "WishlistItems");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "WishlistItems",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "WishlistItems");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "WishlistItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_CategoryId",
                table: "WishlistItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_CategoryWishlists_CategoryId",
                table: "WishlistItems",
                column: "CategoryId",
                principalTable: "CategoryWishlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
