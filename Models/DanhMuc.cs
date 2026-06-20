using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManager.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; } // Sẽ tự động tăng (Identity) trong SQL Server

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string? TenDanhMuc { get; set; }

        public string? MoTa { get; set; }

        // Navigation Property: Thể hiện mối quan hệ 1-N với SanPham
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}