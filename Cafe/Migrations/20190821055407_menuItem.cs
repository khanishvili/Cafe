using Microsoft.EntityFrameworkCore.Migrations;

namespace Cafe.Migrations
{
    public partial class menuItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Taste = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    CategoryID = table.Column<int>(nullable: false),
                    SubCategoryID = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MenuItems_Categoris_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categoris",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MenuItems_SubCategories_ID",
                        column: x => x.ID,
                        principalTable: "SubCategories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryID",
                table: "MenuItems",
                column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems");
        }
    }
}
