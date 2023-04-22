using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTourDuLichAPI.InputModelsApi;
using QLTourDuLichAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace QLTourDuLichAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //nhân non tay
    public class ApiNhanVien : Controller
    {
        QltourdlApiContext db = new QltourdlApiContext();
        [HttpGet]
        public IActionResult GetAllNhanvien()
        {
            var query = (from a in db.NhanViens
                         select new
                         {
                             a.MaNv,
                             a.TenNv,
                             a.GioiTinh,
                             a.SoDienThoai,
                             a.DiaChi,
                             a.UserName
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
        public IActionResult GetAllNhanvienPagination([Range(1, 100)] int pageSize,
           [Range(1, int.MaxValue)] int pageNumber)
        {
            var listNV = (from a in db.NhanViens
                          select new
                          {
                              a.MaNv,
                              a.TenNv,
                              a.GioiTinh,
                              a.SoDienThoai,
                              a.DiaChi,
                              a.UserName
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
        public IActionResult GetNhanVienId(string id)
        {
            var NV = (from a in db.NhanViens
                       where a.MaNv == id
                       select new
                       {
                           a.MaNv,
                           a.TenNv,
                           a.GioiTinh,
                           a.SoDienThoai,
                           a.DiaChi,
                           a.UserName
                       })
                              .FirstOrDefault();
            return Ok(NV);
        }
        [HttpPost]
        [Route("themNV")]
        public async Task<IActionResult> AddNV([FromForm] NhanVienCreateInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var DDCheck = db.NhanViens.Select(x => x.MaNv).ToList();
            if (DDCheck.Any(x => x.Contains(input.MaNV)))
            {
                return BadRequest("Đã Tồn Tại Nhan Vien!");
            }

            var newNV = new NhanVien
            {
                MaNv = input.MaNV,
                TenNv = input.TenNV,
                GioiTinh = input.GioiTinh,
                SoDienThoai = input.SoDienThoai,
                DiaChi = input.DiaChi,
                UserName = input.UserName
                
            };

            db.NhanViens.Add(newNV);
            db.SaveChanges();
            return Ok(input);
        }

        [HttpPut]
        [Route("capnhatNV")]
        public async Task<IActionResult> UpdateNVAsync([FromForm] NhanVienCreateInputModel input)
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
            var NV = await db.NhanViens.FindAsync(input.MaNV);
            if (NV == null)
            {
                return NotFound();
            }

            // Update the TinTuc object with the form data
            NV.MaNv = input.MaNV;
            NV.TenNv = input.TenNV;
            NV.GioiTinh = input.GioiTinh;
            NV.SoDienThoai = input.SoDienThoai;
            NV.DiaChi = input.DiaChi;
            NV.UserName = input.UserName;



            // Update the TinTuc in the database
            db.NhanViens.Update(NV);
            await db.SaveChangesAsync();

            return Ok();
        }
        
        [HttpDelete]
        public IActionResult DeleteNV(string input)
        {
            var DDCheck = (from a in db.NhanViens
                           where a.MaNv == input
                           select a).FirstOrDefault();

            if (DDCheck == null)
            {
                return BadRequest("Khong tim thay nhan vien!");
            }

            db.NhanViens.Remove(DDCheck);
            db.SaveChanges();

            return Ok(input);
        }
    }
}
