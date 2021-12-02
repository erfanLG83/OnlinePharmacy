using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlinePharmacy.Data.Migrations
{
    public partial class Rename_Fields_ProductsTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecommendationsAndWarningsAsJson",
                table: "Products",
                newName: "RecommendationsAndWarningsList");

            migrationBuilder.RenameColumn(
                name: "PropertiesListAsJson",
                table: "Products",
                newName: "PropertiesList");

            migrationBuilder.RenameColumn(
                name: "CompoundsAsJson",
                table: "Products",
                newName: "CompoundsList");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecommendationsAndWarningsList",
                table: "Products",
                newName: "RecommendationsAndWarningsAsJson");

            migrationBuilder.RenameColumn(
                name: "PropertiesList",
                table: "Products",
                newName: "PropertiesListAsJson");

            migrationBuilder.RenameColumn(
                name: "CompoundsList",
                table: "Products",
                newName: "CompoundsAsJson");
        }
    }
}
