using System.ComponentModel.DataAnnotations;

namespace CoffeeShopManager.Models
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100)]
        public string HoTen { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(150)]
        public string Email { get; set; } = "";

        [Required]
        public string MatKhauHash { get; set; } = "";

        public string VaiTro { get; set; } = "User";
    }
}
