using CoffeeShopManager.Data;
using CoffeeShopManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CoffeeShopManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NguoiDungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NguoiDungController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int LayMaAdminHienTai()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int maNguoiDung))
            {
                return maNguoiDung;
            }

            return 0;
        }

        private List<string> DanhSachVaiTro()
        {
            return new List<string>
            {
                "User",
                "Admin"
            };
        }

        // ADMIN: Xem danh sách người dùng
        public async Task<IActionResult> Index()
        {
            var nguoiDungs = await _context.NguoiDungs
                .OrderBy(n => n.VaiTro)
                .ThenBy(n => n.HoTen)
                .ToListAsync();

            ViewBag.CurrentAdminId = LayMaAdminHienTai();

            return View(nguoiDungs);
        }

        // ADMIN: Xem chi tiết người dùng
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(n => n.MaNguoiDung == id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            var soDonHang = await _context.DonHangs
                .CountAsync(d => d.MaNguoiDung == nguoiDung.MaNguoiDung);

            ViewBag.SoDonHang = soDonHang;

            return View(nguoiDung);
        }

        // ADMIN: Form sửa người dùng
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs.FindAsync(id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            ViewBag.DanhSachVaiTro = DanhSachVaiTro();
            ViewBag.CurrentAdminId = LayMaAdminHienTai();

            return View(nguoiDung);
        }

        // ADMIN: Xử lý sửa người dùng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string hoTen, string vaiTro)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            ViewBag.DanhSachVaiTro = DanhSachVaiTro();
            ViewBag.CurrentAdminId = LayMaAdminHienTai();

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                ModelState.AddModelError("", "Vui lòng nhập họ tên.");
                return View(nguoiDung);
            }

            if (!DanhSachVaiTro().Contains(vaiTro))
            {
                ModelState.AddModelError("", "Vai trò không hợp lệ.");
                return View(nguoiDung);
            }

            int maAdminHienTai = LayMaAdminHienTai();

            if (nguoiDung.MaNguoiDung == maAdminHienTai && vaiTro != "Admin")
            {
                ModelState.AddModelError("", "Bạn không thể tự đổi tài khoản Admin của mình thành User.");
                return View(nguoiDung);
            }

            if (nguoiDung.VaiTro == "Admin" && vaiTro != "Admin")
            {
                var soAdminKhac = await _context.NguoiDungs
                    .CountAsync(n => n.VaiTro == "Admin" && n.MaNguoiDung != nguoiDung.MaNguoiDung);

                if (soAdminKhac == 0)
                {
                    ModelState.AddModelError("", "Không thể đổi vai trò vì hệ thống phải còn ít nhất một tài khoản Admin.");
                    return View(nguoiDung);
                }
            }

            nguoiDung.HoTen = hoTen.Trim();
            nguoiDung.VaiTro = vaiTro;

            _context.Update(nguoiDung);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật người dùng thành công.";

            return RedirectToAction(nameof(Index));
        }

        // ADMIN: Form xóa người dùng
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs
                .FirstOrDefaultAsync(n => n.MaNguoiDung == id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            var soDonHang = await _context.DonHangs
                .CountAsync(d => d.MaNguoiDung == nguoiDung.MaNguoiDung);

            ViewBag.SoDonHang = soDonHang;
            ViewBag.CurrentAdminId = LayMaAdminHienTai();

            return View(nguoiDung);
        }

        // ADMIN: Xử lý xóa người dùng
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            int maAdminHienTai = LayMaAdminHienTai();

            if (nguoiDung.MaNguoiDung == maAdminHienTai)
            {
                TempData["Error"] = "Bạn không thể tự xóa tài khoản đang đăng nhập.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            var soDonHang = await _context.DonHangs
                .CountAsync(d => d.MaNguoiDung == nguoiDung.MaNguoiDung);

            if (soDonHang > 0)
            {
                TempData["Error"] = "Không thể xóa người dùng này vì đã có đơn hàng trong hệ thống.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            if (nguoiDung.VaiTro == "Admin")
            {
                var soAdminKhac = await _context.NguoiDungs
                    .CountAsync(n => n.VaiTro == "Admin" && n.MaNguoiDung != nguoiDung.MaNguoiDung);

                if (soAdminKhac == 0)
                {
                    TempData["Error"] = "Không thể xóa vì hệ thống phải còn ít nhất một tài khoản Admin.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }

            _context.NguoiDungs.Remove(nguoiDung);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Xóa người dùng thành công.";

            return RedirectToAction(nameof(Index));
        }
    }
}