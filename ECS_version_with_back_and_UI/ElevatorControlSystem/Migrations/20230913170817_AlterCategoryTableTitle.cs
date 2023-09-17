using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevatorCS_ef.Migrations
{
    /// <inheritdoc />
    public partial class AlterCategoryTableTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "categories",
                newName: "depotItemCategories");

            migrationBuilder.RenameIndex(
                name: "IX_categories_depotItem_id",
                table: "depotItemCategories",
                newName: "IX_depotItemCategories_depotItem_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "depotItemCategories",
                newName: "categories");

            migrationBuilder.RenameIndex(
                name: "IX_depotItemCategories_depotItem_id",
                table: "categories",
                newName: "IX_categories_depotItem_id");
        }
    }
}
