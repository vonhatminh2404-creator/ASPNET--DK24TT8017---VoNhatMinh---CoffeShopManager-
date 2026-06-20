using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShopManager.Migrations
{
    /// <inheritdoc />
    public partial class CapNhatDongBoCuoiCung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SanPhamMaSP",
                table: "DonHangs",
                newName: "MaSP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaSP",
                table: "DonHangs",
                newName: "SanPhamMaSP");
        }
    }
}
