using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class efcoreinit3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Periority",
                schema: "PPP",
                table: "DefinedProductImage");

            migrationBuilder.AddColumn<bool>(
                name: "IsCover",
                schema: "PPP",
                table: "DefinedProductImage",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCover",
                schema: "PPP",
                table: "DefinedProductImage");

            migrationBuilder.AddColumn<int>(
                name: "Periority",
                schema: "PPP",
                table: "DefinedProductImage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
