using CoffeeShopManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.ToTable("SanPhams");
                entity.HasKey(e => e.MaSP);
                entity.Property(e => e.MaSP).ValueGeneratedOnAdd();

                entity.Property(e => e.Gia)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.Property(e => e.GiaBan)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.ToTable("NguoiDungs");
                entity.HasKey(e => e.MaNguoiDung);

                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });
        }
    }
}