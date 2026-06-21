
using CoffeeShopManager.Data;
using CoffeeShopManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: SANPHAMS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.SanPhams.ToListAsync());
    }

    // GET: SANPHAMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        // Tìm món ăn theo ID, đồng thời Include (bao gồm) luôn cả bảng DanhMuc
        var sanPham = await _context.SanPhams
            .Include(s => s.DanhMuc)
            .FirstOrDefaultAsync(m => m.MaSP == id);

        if (sanPham == null)
        {
            return NotFound();
        }

        return View(sanPham);
    }

    // GET: SANPHAMS/Create
    public IActionResult Create()
    {
        // Truyền danh sách thả xuống sang giao diện với tên đúng là DanhSachMenu
        ViewBag.DanhSachMenu = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc");

        return View();
    }

    // 2. Hàm POST: Chạy khi người dùng bấm nút "XÁC NHẬN ĐĂNG KÝ"
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SanPham sanPham)
    {
        // Bỏ qua kiểm tra thực thể Danh mục để tránh lỗi liên kết
        ModelState.Remove("DanhMuc");

        if (ModelState.IsValid)
        {
            // TUYỆT CHIÊU CUỐI: Tự tay đóng gói một sản phẩm hoàn toàn mới.
            // Tuyệt đối không đụng đến MaSP, ép hệ thống phải để trống cho SQL Server tự đếm.
            var monMoi = new SanPham
            {
                TenSP = sanPham.TenSP,
                Gia = sanPham.Gia,
                HinhAnh = sanPham.HinhAnh,
                MaDanhMuc = sanPham.MaDanhMuc
            };

            _context.Add(monMoi);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Thành công, trở về trang chủ Admin
        }

        // Nạp lại danh sách nếu bị lỗi nhập liệu
        ViewBag.DanhSachMenu = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
        return View(sanPham);
    }

    // GET: SANPHAMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        // Tìm món ăn theo Mã SP
        var sanPham = await _context.SanPhams.FindAsync(id);
        if (sanPham == null) return NotFound();

        // Nạp danh sách Dropdown và chọn sẵn danh mục cũ của món ăn đó
        ViewBag.DanhSachMenu = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
        return View(sanPham);
    }

    // 2. Hàm POST: Chạy khi bấm nút "LƯU THAY ĐỔI"
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SanPham sanPham)
    {
        if (id != sanPham.MaSP) return NotFound();

        // BẮT BUỘC: Bỏ qua kiểm tra Danh Mục để không bị lỗi oan (y hệt lúc Thêm)
        ModelState.Remove("DanhMuc");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(sanPham);
                await _context.SaveChangesAsync(); // Lưu đè vào SQL Server
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(sanPham.MaSP)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index)); // Thành công thì quay về trang kho
        }

        // Nạp lại danh sách nếu bị lỗi nhập liệu
        ViewBag.DanhSachMenu = new SelectList(_context.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
        return View(sanPham);
    }

    // Hàm phụ trợ để kiểm tra xem sản phẩm có tồn tại không
    private bool SanPhamExists(int id)
    {
        return _context.SanPhams.Any(e => e.MaSP == id);
    }

    // GET: SANPHAMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        // Tìm sản phẩm và lấy luôn cả thông tin Danh mục đi kèm để hiển thị
        var sanPham = await _context.SanPhams
            .Include(s => s.DanhMuc)
            .FirstOrDefaultAsync(m => m.MaSP == id);

        if (sanPham == null) return NotFound();

        return View(sanPham);
    }

    // 2. Hàm POST: Lệnh thực thi xóa thật sự vào SQL Server
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var sanPham = await _context.SanPhams.FindAsync(id);
        if (sanPham != null)
        {
            // Ra lệnh xóa sản phẩm khỏi cơ sở dữ liệu
            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index)); // Xóa xong thì quay về kho lưu trữ
    }


        private bool SanPhamExists(int? masp)
    {
        return _context.SanPhams.Any(e => e.MaSP == masp);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AnHien(int id)
    {
        var sanPham = await _context.SanPhams.FindAsync(id);

        if (sanPham == null)
        {
            return NotFound();
        }

        sanPham.IsAn = !sanPham.IsAn;

        _context.Update(sanPham);
        await _context.SaveChangesAsync();

        TempData["Success"] = sanPham.IsAn
            ? "Sản phẩm đã được ẩn khỏi trang người dùng."
            : "Sản phẩm đã được hiển thị lại trên trang người dùng.";

        return RedirectToAction(nameof(Details), new { id = id });
    }
}
