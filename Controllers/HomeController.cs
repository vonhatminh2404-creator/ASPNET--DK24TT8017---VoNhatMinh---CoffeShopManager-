using CoffeeShopManager.Data;
using CoffeeShopManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CoffeeShopManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? keyword,
            int? danhMucId,
            decimal? giaTu,
            decimal? giaDen)
        {
            var query = _context.SanPhams
                .Include(s => s.DanhMuc)
                .AsQueryable();

            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s => s.TenSP != null && s.TenSP.Contains(keyword));
            }

            // Lọc theo danh mục
            if (danhMucId.HasValue && danhMucId.Value > 0)
            {
                query = query.Where(s => s.MaDanhMuc == danhMucId.Value);
            }

            // Lọc giá từ
            if (giaTu.HasValue)
            {
                query = query.Where(s => s.Gia >= giaTu.Value);
            }

            // Lọc giá đến
            if (giaDen.HasValue)
            {
                query = query.Where(s => s.Gia <= giaDen.Value);
            }

            var danhSachSanPham = await query
                .OrderBy(s => s.MaDanhMuc)
                .ThenBy(s => s.TenSP)
                .ToListAsync();

            var danhMucs = await _context.DanhMucs
                .OrderBy(d => d.TenDanhMuc)
                .ToListAsync();

            ViewBag.Keyword = keyword;
            ViewBag.DanhMucId = danhMucId;
            ViewBag.GiaTu = giaTu;
            ViewBag.GiaDen = giaDen;

            ViewBag.DanhMucSelectList = new SelectList(
                danhMucs,
                "MaDanhMuc",
                "TenDanhMuc",
                danhMucId
            );

            return View(danhSachSanPham);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}