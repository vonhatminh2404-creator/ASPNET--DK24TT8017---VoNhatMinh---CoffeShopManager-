using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShopManager.Models
{
    public class SanPham
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200)]
        public string? TenSP { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Giá phải là số dương")]
        public decimal Gia { get; set; }

        public string? HinhAnh { get; set; }

        public int MaDanhMuc { get; set; }

        [ForeignKey("MaDanhMuc")]
        public virtual DanhMuc? DanhMuc { get; set; }

        // false = đang hiển thị, true = đang ẩn
        public bool IsAn { get; set; } = false;
    }
}