using Microsoft.EntityFrameworkCore;
using CoffeeShopManager.Models;

namespace CoffeeShopManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Hàm khởi tạo nhận cấu hình kết nối từ bên ngoài truyền vào
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình tường minh: Ép C# phải tuân theo cấu trúc của SQL Server
            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.ToTable("SanPhams"); // Khớp chính xác tên bảng (có chữ s) trong SQL
                entity.HasKey(e => e.MaSP); // Khẳng định MaSP là Khóa chính
                entity.Property(e => e.MaSP).ValueGeneratedOnAdd(); // LỆNH TỐI CAO: Bắt buộc tự đếm
            });
        }

        // Khai báo các bảng sẽ xuất hiện trong SQL Server
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<CoffeeShopManager.Models.DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }


    }
}