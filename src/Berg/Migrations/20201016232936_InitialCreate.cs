using Microsoft.EntityFrameworkCore.Migrations;

namespace Berg.Migrations {
    public partial class InitialCreate : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Item", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "Item");
        }
    }
}
