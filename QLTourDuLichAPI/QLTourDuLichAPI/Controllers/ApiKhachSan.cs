using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTourDuLichAPI.InputModelsApi;
using QLTourDuLichAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace QLTourDuLichAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKhachSan : Controller
    {
        QltourdlApiContext db = new QltourdlApiContext();
        [HttpGet]
        public IActionResult getAllKhachsan()
        {
            var query = (from a in db.KhachSans
                         select new
                         {
                             a.MaKs,
                             a.TenKs,
                             a.DiaChi,
                             a.Sdt,
                             a.XepHangKs,
                             a.TenFileAnh
                         });
            var totalCount = query.Count();
            //var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var listKS = query
                .ToList();

            var result = new
            {
                TotalCount = totalCount,
                //PageCount = pageCount,
                Items = listKS
            };

            return Ok(result);

        }
        [HttpGet]
        [Route("getPagination")]
        public IActionResult GetAllKhachsanPagination([Range(1, 100)] int pageSize,
           [Range(1, int.MaxValue)] int pageNumber)
        {
            var listKS = (from a in db.KhachSans
                          select new
                          {
                              a.MaKs,
                              a.TenKs,
                              a.DiaChi,
                              a.Sdt,
                              a.XepHangKs,
                              a.TenFileAnh
                          })
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
            var result = new
            {
                Items = listKS
            };
            return Ok(result);
        }
        [Route("getById")]
        [HttpGet]
        public IActionResult GetKhachSanId(string id)
        {
            var KS = (from a in db.KhachSans
                      where a.MaKs == id
                      select new
                      {
                          a.MaKs,
                          a.TenKs,
                          a.DiaChi,
                          a.Sdt,
                          a.XepHangKs,
                          a.TenFileAnh
                      })
                              .FirstOrDefault();
            return Ok(KS);
        }
        [HttpPost]
        [Route("themKS")]
        public async Task<IActionResult> AddKS([FromForm] KhachSanCreateInputMode input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Upload the image to the server
            string fileName = await UploadImage(input.TenFileAnh);
            var DDCheck = db.KhachSans.Select(x => x.MaKs).ToList();
            if (DDCheck.Any(x => x.Contains(input.MaKs)))
            {
                return BadRequest("Đã Tồn Tại Điểm Thăm Quan!");
            }

            var newKS = new KhachSan
            {
                MaKs = input.MaKs,
                TenKs = input.TenKs,
                DiaChi = input.DiaChi,
                Sdt = input.Sdt,
                XepHangKs = input.XepHangKs,
                TenFileAnh = fileName
            };

            db.KhachSans.Add(newKS);
            db.SaveChanges();
            return Ok(input);
        }

        [HttpPut]
        [Route("capnhatKS")]
        public async Task<IActionResult> UpdateKSAsync([FromForm] KhachSanCreateInputMode input)
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

            // Find the KhachSan in the database by id
            var KS = await db.KhachSans.FindAsync(input.MaKs);
            if (KS == null)
            {
                return NotFound();
            }

            // Update the KhachSan object with the form data
            KS.MaKs = input.MaKs;
            KS.TenKs = input.TenKs;
            KS.DiaChi = input.DiaChi;
            KS.Sdt = input.Sdt;
            KS.XepHangKs = input.XepHangKs;



            // Upload the image to the server and update the KhachSan object with the new image name
            if (input.TenFileAnh != null)
            {
                string fileName = await UploadImage(input.TenFileAnh);
                KS.TenFileAnh = fileName;
            }

            // Update the KhachSan in the database
            db.KhachSans.Update(KS);
            await db.SaveChangesAsync();

            return Ok();
        }
        private async Task<string> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }
            // Get the file name and extension
            string fileName = file.FileName;
            // Set the file path
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "anhDLTC", fileName);
            // Save the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
        [HttpDelete]
        public IActionResult DeleteKS(string input)
        {
            var DDCheck = (from a in db.KhachSans
                           where a.MaKs == input
                           select a).FirstOrDefault();
            var check = (from a in db.KhachSans
                         join b in db.KhachSanTours on a.MaKs equals b.MaKs
                         where b.MaKs == input
                         select a).FirstOrDefault();
            if (DDCheck == null)
            {
                return BadRequest("Khong tim thay diem tham quan!");
            }
            else if (check != null)
            {
                return BadRequest("Khong thể xóa");
            }
            db.KhachSans.Remove(DDCheck);
            db.SaveChanges();
            return Ok(input);
        }
    }
}
