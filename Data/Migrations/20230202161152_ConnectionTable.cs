using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karrot.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConnectionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Products",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CartItems",
                newName: "CartItemProductId");

            migrationBuilder.AddColumn<string>(
                name: "RatingUserId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderItemUserId",
                table: "OrderItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommentProductId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CommentUserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CartItemUserId",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RatingUserId",
                table: "Ratings",
                column: "RatingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OwnerId",
                table: "Products",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderUserId",
                table: "Orders",
                column: "OrderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderItemUserId",
                table: "OrderItems",
                column: "OrderItemUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentProductId",
                table: "Comments",
                column: "CommentProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentUserId",
                table: "Comments",
                column: "CommentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartItemProductId",
                table: "CartItems",
                column: "CartItemProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartItemUserId",
                table: "CartItems",
                column: "CartItemUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AspNetUsers_CartItemUserId",
                table: "CartItems",
                column: "CartItemUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_CartItemProductId",
                table: "CartItems",
                column: "CartItemProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CommentUserId",
                table: "Comments",
                column: "CommentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Products_CommentProductId",
                table: "Comments",
                column: "CommentProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_AspNetUsers_OrderItemUserId",
                table: "OrderItems",
                column: "OrderItemUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_OrderUserId",
                table: "Orders",
                column: "OrderUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_OwnerId",
                table: "Products",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_RatingUserId",
                table: "Ratings",
                column: "RatingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AspNetUsers_CartItemUserId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_CartItemProductId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CommentUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Products_CommentProductId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_AspNetUsers_OrderItemUserId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_OrderUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_OwnerId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_RatingUserId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_RatingUserId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OwnerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderItemUserId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CommentProductId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CommentUserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartItemProductId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartItemUserId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "RatingUserId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderItemUserId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CommentProductId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CartItemUserId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Products",
                newName: "CategoryID");

            migrationBuilder.RenameColumn(
                name: "CartItemProductId",
                table: "CartItems",
                newName: "ProductId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
