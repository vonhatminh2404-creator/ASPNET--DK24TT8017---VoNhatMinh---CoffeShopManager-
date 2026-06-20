using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShopManager.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaChiTiet { get; set; }

        // Khóa ngoại liên kết tới DonHang
        public int MaDon { get; set; }
        [ForeignKey("MaDon")]
        public virtual DonHang DonHang { get; set; }

        // Khóa ngoại liên kết tới SanPham
        public int MaSP { get; set; }
        [ForeignKey("MaSP")]
        public virtual SanPham SanPham { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public decimal GiaBan { get; set; } // Giá lúc mua (phòng trường hợp sau này giá gốc của SanPham thay đổi)
    }
}