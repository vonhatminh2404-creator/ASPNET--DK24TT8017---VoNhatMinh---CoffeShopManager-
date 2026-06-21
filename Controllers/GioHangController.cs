using CoffeeShopManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopManager.Controllers
{
    [Authorize]
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemVaoGio(int id)
        {
            var sanPham = await _context.SanPhams
                .FirstOrDefaultAsync(s => s.MaSP == id && !s.IsAn);

            if (sanPham == null)
            {
                return NotFound();
            }

            var gioHang = HttpContext.Session.GetString("GioHang") ?? "";

            var danhSachId = string.IsNullOrWhiteSpace(gioHang)
                ? new List<int>()
                : gioHang.Split(',').Select(int.Parse).ToList();

            danhSachId.Add(id);

            HttpContext.Session.SetString("GioHang", string.Join(",", danhSachId));

            TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng.";

            return RedirectToAction("Details", "Home", new { id = id });
        }
    }
}