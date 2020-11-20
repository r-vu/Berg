using Microsoft.EntityFrameworkCore.Migrations;

namespace Berg.Migrations
{
    public partial class AddAverageRatingAndNormalizeItemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                schema: "Berg",
                table: "Item",
                newName: "Id");

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                schema: "Berg",
                table: "Item",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                schema: "Berg",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Berg",
                table: "Item",
                newName: "ID");
        }
    }
}
