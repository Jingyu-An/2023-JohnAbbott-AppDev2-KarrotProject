using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karrot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RatingIdAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "RatingId2",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "RatedSellerId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "RatingId2");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RatedSellerId",
                table: "Ratings",
                column: "RatedSellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedSellerId",
                table: "Ratings",
                column: "RatedSellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedSellerId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_RatedSellerId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RatingId2",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RatedSellerId",
                table: "Ratings");

            migrationBuilder.AddColumn<string>(
                name: "RatingId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "RatingId");
        }
    }
}
