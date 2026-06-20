using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShopManager.Data; // Kéo thư mục chứa ApplicationDbContext vào
using CoffeeShopManager.Models;
using System.Diagnostics;

namespace CoffeeShopManager.Controllers
{
    public class HomeController : Controller
    {
        // 1. Khai báo biến kết nối cơ sở dữ liệu
        private readonly ApplicationDbContext _context;

        // 2. Hàm khởi tạo để hệ thống tự động truyền CSDL vào HomeController
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 3. Sửa lại hàm Index để lấy dữ liệu thực tế từ SQL Server
        public async Task<IActionResult> Index()
        {
            // Lấy toàn bộ sản phẩm, bắt buộc kèm theo lệnh .Include để nạp tên Danh Mục
            var danhSachSanPham = await _context.SanPhams
                                        .Include(s => s.DanhMuc)
                                        .ToListAsync();

            // Truyền danh sách thực tế này ra ngoài giao diện trang chủ
            return View(danhSachSanPham);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}