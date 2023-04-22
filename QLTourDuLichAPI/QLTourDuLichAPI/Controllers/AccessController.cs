using Microsoft.AspNetCore.Mvc;
using QLTourDuLichAPI.Models;


namespace QLTourDuLichAPI.Controllers
{
    public class AccessController : Controller
    {
        QltourdlApiContext db = new QltourdlApiContext();

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"))){
                return RedirectToAction("Login");
            }
            ViewBag.Username = HttpContext.Session.GetString("UserName");
            ViewBag.Role = HttpContext.Session.GetString("Loai");
            return View();
        }
        [HttpPost]
        public IActionResult Login(TaiKhoan taiKhoan, int admin = 1, int user = 0)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var tk1 = db.TaiKhoans.Where(x => x.UserName.Equals(taiKhoan.UserName) && x.Password.Equals(taiKhoan.Password) && x.Loai.Equals(0)).ToList().FirstOrDefault();
                var tk2 = db.TaiKhoans.Where(x => x.UserName.Equals(taiKhoan.UserName) && x.Password.Equals(taiKhoan.Password) && x.Loai.Equals(1)).ToList().FirstOrDefault();
                //var l = db.TaiKhoans.Count(m => m.Loai == loai);

                if (tk1 != null)
                {

                    HttpContext.Session.SetString("UserName", tk1.UserName.ToString());
                    return Redirect("~/Home/Index");
                }
                if (tk2 != null)
                {

                    HttpContext.Session.SetString("UserName", tk2.UserName.ToString());
                    return Redirect("~/Admin/Index");
                }
            }

            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register(TaiKhoan user)
        {
            if (ModelState.IsValid)
            {
                var check = db.TaiKhoans.Where(x => x.UserName.Equals(user.UserName) && x.Loai.Equals(2)).ToList().FirstOrDefault();
                if (check == null)
                {
                    //user.Password = getMD5(user.Password);                 
                    db.TaiKhoans.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Access");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
    }
}
