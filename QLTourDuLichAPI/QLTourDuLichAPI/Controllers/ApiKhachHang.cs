using Microsoft.AspNetCore.Mvc;
using QLTourDuLichAPI.InputModelsApi;
using QLTourDuLichAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace QLTourDuLichAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKhachHang : Controller
    {
        QltourdlApiContext db = new QltourdlApiContext();
        public IActionResult GetAllKhachHang()
        {
            var query = (from a in db.KhachHangs
                         select new
                         {
                             a.MaKh,
                             a.TenKh,
                             a.GioiTinh,
                             a.Sdt,
                             a.DiaChi,
                             a.UserName,
                             a.Email
                         });
            var totalCount = query.Count();
            //var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var listNV = query
                .ToList();

            var result = new
            {
                TotalCount = totalCount,
                //PageCount = pageCount,
                Items = listNV
            };

            return Ok(result);

        }
        [HttpGet]
        [Route("getPagination")]
        public IActionResult GetAllKhachHangPagination([Range(1, 100)] int pageSize,
           [Range(1, int.MaxValue)] int pageNumber)
        {
            var listNV = (from a in db.KhachHangs
                          select new
                          {
                              a.MaKh,
                              a.TenKh,
                              a.GioiTinh,
                              a.Sdt,
                              a.DiaChi,
                              a.UserName,
                              a.Email
                          })
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
            var result = new
            {
                Items = listNV
            };
            return Ok(result);
        }
        [Route("getById")]
        [HttpGet]
        public IActionResult GetKhachHangId(string id)
        {
            var NV = (from a in db.KhachHangs
                      where a.MaKh == id
                      select new
                      {
                          a.MaKh,
                          a.TenKh,
                          a.GioiTinh,
                          a.Sdt,
                          a.DiaChi,
                          a.UserName,
                          a.Email
                      })
                              .FirstOrDefault();
            return Ok(NV);
        }
        [HttpPost]
        [Route("themKH")]
        public async Task<IActionResult> AddKH([FromForm] KhachHangCreateInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var KHCheck = db.KhachHangs.Select(x => x.MaKh).ToList();
            if (KHCheck.Any(x => x.Contains(input.MaKH)))
            {
                return BadRequest("Đã Tồn Tại Khách Hàng!");
            }

            var newKH = new KhachHang
            {
                MaKh = input.MaKH,
                TenKh = input.TenKH,
                GioiTinh = input.GioiTinh,
                Sdt = input.Sdt,
                DiaChi = input.DiaChi,
                UserName = input.UserName,
                Email = input.Email,
            };

            db.KhachHangs.Add(newKH);
            db.SaveChanges();
            return Ok(input);
        }

        [HttpPut]
        [Route("capnhatKH")]
        public async Task<IActionResult> UpdateNVAsync([FromForm] KhachHangUpdateInputModel input)
        {
            ViewBag.Username = HttpContext.Session.GetString("UserName");
            var username = HttpContext.Session.GetString("UserName");
            var userid = (from a in db.TaiKhoans
                          where a.UserName == username
                          select a.UserName.ToString()).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the DiemThamQuan in the database by id
            var KH = await db.KhachHangs.FindAsync(input.MaKH);
            if (KH == null)
            {
                return NotFound();
            }

            // Update the TinTuc object with the form data
            KH.MaKh = input.MaKH;
            KH.TenKh = input.TenKH;
            KH.GioiTinh = input.GioiTinh;
            KH.Sdt = input.Sdt;
            KH.DiaChi = input.DiaChi;
            KH.UserName = input.UserName;
            KH.Email = input.Email;


            // Update the TinTuc in the database
            db.KhachHangs.Update(KH);
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteKH(string input)
        {
            var KHCheck = (from a in db.KhachHangs
                           where a.MaKh == input
                           select a).FirstOrDefault();

            if (KHCheck == null)
            {
                return BadRequest("Khong tim thay Khách Hàng!");
            }

            db.KhachHangs.Remove(KHCheck);
            db.SaveChanges();

            return Ok(input);
        }
    }
}
