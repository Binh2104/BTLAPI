using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTourDuLichAPI.Models;
using System.Diagnostics;
using X.PagedList;

namespace QLTourDuLichAPI.Controllers
{
    public class HomeController : Controller
    {
        QltourdlApiContext db = new QltourdlApiContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /*public List<Tour> ListTour(string idTour, string nameTour, int gia, string anh, string dxp,int star,int NgayDi)
		{
			TinTuc= db.Tours.Where(x=>x.idTour)
			return ();
		}*/
        public IActionResult DiemDen(int page = 1)
        {
            int pageNumber = page;
            int pageSize = 3;
            var lstsanpham = db.Tours.OrderBy(x => x.TenTour).ToList();
            PagedList<Tour> lst = new PagedList<Tour>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
		public IActionResult KhachSan(int page = 1)
		{
			int pageNumber = page;
			int pageSize = 3;
			var lstsanpham = db.KhachSans.OrderBy(x => x.TenKs).ToList();
			PagedList<KhachSan> lst = new PagedList<KhachSan>(lstsanpham, pageNumber, pageSize);
			return View(lst);
		}
        public IActionResult ChiTietKhachSan (string maKhachSan) {
			var KhachSan = db.KhachSans.SingleOrDefault(x => x.MaKs== maKhachSan);

            var chi = (from a in db.KhachSans
                       where a.MaKs == maKhachSan
                       select a
                     ).ToList();
            ViewBag.chitietKs = chi;

			return View(KhachSan);
		}
		public IActionResult ChiTietTour(string MaTour)
        {
            var chitiet = (from a in db.DiemThamQuans
                           join b in db.DiaDiemTours on a.MaDd equals b.MaDd
                           join c in db.Tours on b.MaTour equals c.MaTour
                           where b.MaTour == MaTour
                           select new
                           {
                               a.Tendiadiem,
                               c.Anh,
                               c.SoNgayDl,
                               c.DiemXuatPhat,
                               c.XepHangTour,
                               a.TenFileAnh,
                               c.TenTour,
                               a.MoTa,
                               c.GiaCho,
                               a.Mien,
                           }
                                ).ToList();
            var chi = (from a in db.Tours
                       where a.MaTour == MaTour
                       select a
                     ).ToList();
            ViewBag.chi = chi;
            ViewBag.chitiet = chitiet;
            return View(chitiet);
            /*return View(chitiet);*/
        }
        public IActionResult Index()
        {
            var top5tour = (from a in db.Tours where a.XepHangTour == 5 select a).Take(5).ToList();
            return View(top5tour);
        }
        public IActionResult ThuVien(int page = 1)
        {
            int pageNumber = page;
            int pageSize = 8;
            var lsthinhanh = db.DiemThamQuans.OrderBy(x => x.MaDd).ToList();
            PagedList<DiemThamQuan> lst = new PagedList<DiemThamQuan>(lsthinhanh, pageNumber, pageSize);
            return View(lst);
            /*  var lstsanpham = db.Dia.OrderBy(x => x.TenFileAnh).ToList();
			  PagedList<AnhDlTc> lst = new PagedList<AnhDlTc>(lstsanpham, pageNumber, pageSize);
			  return View(lst);*/
        }
        public IActionResult TinTuc(int page = 1)
        {

            int pageSize = 5;
            int pageNumber = page;
            var listSP = db.TinTucs.AsNoTracking().OrderBy(x => x.MaTin);
            PagedList<TinTuc> lst = new PagedList<TinTuc>(listSP, pageNumber, pageSize);
            var nguoidang = (from a in db.TinTucs
                             join b in db.NhanViens on a.MaNv equals b.MaNv
                             select b.TenNv).ToList();
            ViewBag.nguoidang = nguoidang;
            return View(lst);
        }
        public IActionResult ChiTietTinTuc(String maTin)
        {

            var Tin = db.TinTucs.SingleOrDefault(x => x.MaTin == maTin);
            var anhTin = db.AnhTins.Where(x => x.MaTin == maTin).ToList();
            var tinTuc = db.TinTucs.Where(x => x.MaTin == maTin).ToList();
            ViewBag.anhTin = anhTin;
            ViewBag.tinTuc = tinTuc;
            var listSP = db.TinTucs.AsNoTracking();
            ViewBag.Tint = listSP;

            return View(Tin);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}