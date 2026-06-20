using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShopManager.Migrations
{
    /// <inheritdoc />
    public partial class ChoPhepRongMaSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSP",
                table: "DonHangs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SanPhamMaSP",
                table: "DonHangs");
        }
    }
}
