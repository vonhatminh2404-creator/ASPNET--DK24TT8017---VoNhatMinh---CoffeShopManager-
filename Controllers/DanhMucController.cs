using CoffeeShopManager.Data;
using CoffeeShopManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DanhMucController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhMucController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ADMIN: Xem danh sách danh mục
        public async Task<IActionResult> Index()
        {
            var danhMucs = await _context.DanhMucs
                .Include(d => d.SanPhams)
                .OrderBy(d => d.TenDanhMuc)
                .ToListAsync();

            return View(danhMucs);
        }

        // ADMIN: Xem chi tiết danh mục
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs
                .Include(d => d.SanPhams)
                .FirstOrDefaultAsync(d => d.MaDanhMuc == id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // ADMIN: Form thêm danh mục
        public IActionResult Create()
        {
            return View();
        }

        // ADMIN: Xử lý thêm danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMuc danhMuc)
        {
            ModelState.Remove("SanPhams");

            if (ModelState.IsValid)
            {
                var tenTrung = await _context.DanhMucs
                    .AnyAsync(d => d.TenDanhMuc != null &&
                                   danhMuc.TenDanhMuc != null &&
                                   d.TenDanhMuc.ToLower() == danhMuc.TenDanhMuc.ToLower());

                if (tenTrung)
                {
                    ModelState.AddModelError("TenDanhMuc", "Tên danh mục này đã tồn tại.");
                    return View(danhMuc);
                }

                _context.DanhMucs.Add(danhMuc);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thêm danh mục thành công.";

                return RedirectToAction(nameof(Index));
            }

            return View(danhMuc);
        }

        // ADMIN: Form sửa danh mục
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs.FindAsync(id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // ADMIN: Xử lý sửa danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DanhMuc danhMuc)
        {
            if (id != danhMuc.MaDanhMuc)
            {
                return NotFound();
            }

            ModelState.Remove("SanPhams");

            if (ModelState.IsValid)
            {
                var tenTrung = await _context.DanhMucs
                    .AnyAsync(d => d.MaDanhMuc != danhMuc.MaDanhMuc &&
                                   d.TenDanhMuc != null &&
                                   danhMuc.TenDanhMuc != null &&
                                   d.TenDanhMuc.ToLower() == danhMuc.TenDanhMuc.ToLower());

                if (tenTrung)
                {
                    ModelState.AddModelError("TenDanhMuc", "Tên danh mục này đã tồn tại.");
                    return View(danhMuc);
                }

                try
                {
                    _context.Update(danhMuc);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật danh mục thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucExists(danhMuc.MaDanhMuc))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(danhMuc);
        }

        // ADMIN: Form xóa danh mục
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _context.DanhMucs
                .Include(d => d.SanPhams)
                .FirstOrDefaultAsync(d => d.MaDanhMuc == id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // ADMIN: Xử lý xóa danh mục
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var danhMuc = await _context.DanhMucs
                .Include(d => d.SanPhams)
                .FirstOrDefaultAsync(d => d.MaDanhMuc == id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            if (danhMuc.SanPhams != null && danhMuc.SanPhams.Any())
            {
                TempData["Error"] = "Không thể xóa danh mục này vì đang có sản phẩm thuộc danh mục.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }

            _context.DanhMucs.Remove(danhMuc);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Xóa danh mục thành công.";

            return RedirectToAction(nameof(Index));
        }

        private bool DanhMucExists(int id)
        {
            return _context.DanhMucs.Any(e => e.MaDanhMuc == id);
        }
    }
}