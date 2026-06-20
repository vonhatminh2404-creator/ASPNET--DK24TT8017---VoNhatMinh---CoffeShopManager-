
using CoffeeShopManager.Data;
using CoffeeShopManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class DonHangController : Controller
{
    private readonly ApplicationDbContext _context;

    public DonHangController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: DONHANGS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.DonHangs.ToListAsync());
    }

    // GET: DONHANGS/Details/5
    public async Task<IActionResult> Details(int? madonhang)
    {
        if (madonhang == null)
        {
            return NotFound();
        }

        var donhang = await _context.DonHangs
            .FirstOrDefaultAsync(m => m.MaDonHang == madonhang);
        if (donhang == null)
        {
            return NotFound();
        }

        return View(donhang);
    }

    // GET: DONHANGS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DONHANGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int? sanPhamId)
    {
        var donHangMoi = new DonHang();
        if (sanPhamId != null)
        {
            var sanPham = await _context.SanPhams.FindAsync(sanPhamId);
            if (sanPham != null)
            {
                donHangMoi.MaSP = sanPham.MaSP; // <-- BẠN HÃY THÊM DÒNG NÀY VÀO ĐÂY
                donHangMoi.TongTien = (double)sanPham.Gia;
                donHangMoi.GhiChu = "Món đặt: " + sanPham.TenSP;
            }
        }
        return View(donHangMoi);
    }

    // GET: DONHANGS/Edit/5
    public async Task<IActionResult> Edit(int? madonhang)
    {
        if (madonhang == null)
        {
            return NotFound();
        }

        var donhang = await _context.DonHangs.FindAsync(madonhang);
        if (donhang == null)
        {
            return NotFound();
        }
        return View(donhang);
    }

    // POST: DONHANGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? madonhang, [Bind("MaDonHang,TenKhachHang,SoDienThoai,NgayDat,TongTien,TrangThai,GhiChu")] DonHang donhang)
    {
        if (madonhang != donhang.MaDonHang)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(donhang);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonHangExists(donhang.MaDonHang))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(donhang);
    }

    // GET: DONHANGS/Delete/5
    public async Task<IActionResult> Delete(int? madonhang)
    {
        if (madonhang == null)
        {
            return NotFound();
        }

        var donhang = await _context.DonHangs
            .FirstOrDefaultAsync(m => m.MaDonHang == madonhang);
        if (donhang == null)
        {
            return NotFound();
        }

        return View(donhang);
    }

    // POST: DONHANGS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? madonhang)
    {
        var donhang = await _context.DonHangs.FindAsync(madonhang);
        if (donhang != null)
        {
            _context.DonHangs.Remove(donhang);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DonHangExists(int? madonhang)
    {
        return _context.DonHangs.Any(e => e.MaDonHang == madonhang);
    }
}
