using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class efcoreinit1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPropertyValue_ProductsFilter_ProductsFilterId",
                schema: "PPP",
                table: "ProductPropertyValue");

            migrationBuilder.RenameColumn(
                name: "ProductsFilterId",
                schema: "PPP",
                table: "ProductPropertyValue",
                newName: "SubFilterId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPropertyValue_ProductsFilterId",
                schema: "PPP",
                table: "ProductPropertyValue",
                newName: "IX_ProductPropertyValue_SubFilterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPropertyValue_SubFilter_SubFilterId",
                schema: "PPP",
                table: "ProductPropertyValue",
                column: "SubFilterId",
                principalSchema: "PPP",
                principalTable: "SubFilter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPropertyValue_SubFilter_SubFilterId",
                schema: "PPP",
                table: "ProductPropertyValue");

            migrationBuilder.RenameColumn(
                name: "SubFilterId",
                schema: "PPP",
                table: "ProductPropertyValue",
                newName: "ProductsFilterId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductPropertyValue_SubFilterId",
                schema: "PPP",
                table: "ProductPropertyValue",
                newName: "IX_ProductPropertyValue_ProductsFilterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPropertyValue_ProductsFilter_ProductsFilterId",
                schema: "PPP",
                table: "ProductPropertyValue",
                column: "ProductsFilterId",
                principalSchema: "PPP",
                principalTable: "ProductsFilter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
