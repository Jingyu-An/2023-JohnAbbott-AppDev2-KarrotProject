using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karrot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RatingIdAdjustment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatingId2",
                table: "Ratings",
                newName: "RatingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Ratings",
                newName: "RatingId2");
        }
    }
}
