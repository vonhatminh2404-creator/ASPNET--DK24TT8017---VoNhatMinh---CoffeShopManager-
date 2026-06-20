using Microsoft.EntityFrameworkCore;
using CoffeeShopManager.Models;

namespace CoffeeShopManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.ToTable("SanPhams");
                entity.HasKey(e => e.MaSP);
                entity.Property(e => e.MaSP).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<NguoiDung>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
    }
}