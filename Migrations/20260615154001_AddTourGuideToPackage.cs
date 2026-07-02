using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace newVisitSuadi2026.Migrations
{
    /// <inheritdoc />
    public partial class AddTourGuideToPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TourGuideId",
                table: "Package",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourGuideId",
                table: "Package");
        }
    }
}
