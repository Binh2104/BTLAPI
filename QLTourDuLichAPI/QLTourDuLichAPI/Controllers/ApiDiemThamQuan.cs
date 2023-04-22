using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using QLTourDuLichAPI.InputModelsApi;
using QLTourDuLichAPI.Models;
namespace QLTourDuLichAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiDiemThamQuan : Controller
    {

        QltourdlApiContext db = new QltourdlApiContext();
        [HttpGet]
        public IActionResult getAllDiemThamQuan()
        {
            var query = (from a in db.DiemThamQuans                         
                         select new
                         {
                             a.Tendiadiem,
                             a.MoTa,
                             a.MaDd,
                             a.TenFileAnh,
                             a.Mien
                         });
            var totalCount = query.Count();
            //var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var listDTQ= query
                .ToList();

            var result = new
            {
                TotalCount = totalCount,
                //PageCount = pageCount,
                Items = listDTQ
            };

            return Ok(result);

        }
        [HttpGet]
        [Route("getPagination")]
        public IActionResult GetAllDiemthamquanPagination([Range(1, 100)] int pageSize,
           [Range(1, int.MaxValue)] int pageNumber)
        {
            var listDTQ = (from a in db.DiemThamQuans
                           select new
                           {
                               a.Tendiadiem,
                               a.MoTa,
                               a.MaDd,
                               a.TenFileAnh,
                               a.Mien
                           })
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
            var result = new
            {
                Items = listDTQ
            };
            return Ok(result);
        }
        [Route("getById")]
        [HttpGet]
        public IActionResult GetDiemThamQuanId(string id)
        {
            var DTQ = (from a in db.DiemThamQuans
                       where a.MaDd== id    
                          select new
                          {
                              a.Tendiadiem,
                              a.MoTa,
                              a.MaDd,
                              a.TenFileAnh,
                              a.Mien
                          })
                              .FirstOrDefault();
            return Ok(DTQ);
        }
        [HttpPost]
        [Route("themDTQ")]
        public async Task<IActionResult> AddDTQ([FromForm] DiemThamQuanCreateInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Upload the image to the server
            string fileName = await UploadImage(input.TenFileAnh);
            var DDCheck = db.DiemThamQuans.Select(x => x.MaDd).ToList();
            if (DDCheck.Any(x => x.Contains(input.MaDd)))
            {
                return BadRequest("Đã Tồn Tại Điểm Thăm Quan!");
            }

            var newDTQ = new DiemThamQuan
            {
                MaDd = input.MaDd,
                Tendiadiem = input.Tendiadiem,
                Mien = input.Mien,
                MoTa = input.MoTa,
                TenFileAnh = fileName
            };

            db.DiemThamQuans.Add(newDTQ);
            db.SaveChanges();
            return Ok(input);
        }
       
        [HttpPut]
        [Route("capnhatDTQ")]
        public async Task<IActionResult> UpdateDTQAsync([FromForm] DiemThamQuanCreateInputModel input)
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
            var DTQ = await db.DiemThamQuans.FindAsync(input.MaDd);
            if (DTQ == null)
            {
                return NotFound();
            }

            // Update the TinTuc object with the form data
            DTQ.MaDd = input.MaDd;
            DTQ.Tendiadiem= input.Tendiadiem;
            DTQ.MoTa = input.MoTa;
            DTQ.Mien=input.Mien;
          

            // Upload the image to the server and update the TinTuc object with the new image name
            if (input.TenFileAnh != null)
            {
                string fileName = await UploadImage(input.TenFileAnh);
                DTQ.TenFileAnh = fileName;
            }

            // Update the TinTuc in the database
            db.DiemThamQuans.Update(DTQ);
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
        public IActionResult DeleteDTQ(string input)
        {
            var DDCheck = (from a in db.DiemThamQuans
                           where a.MaDd == input 
                           select a).FirstOrDefault();
            var check = (from a in db.DiemThamQuans
                         join b in db.DiaDiemTours on a.MaDd equals b.MaDd
                        join c in db.Tours on b.MaTour equals c.MaTour
                         where b.MaDd == input
                         select a).FirstOrDefault();
            if (DDCheck == null)
            {
                return BadRequest("Khong tim thay diem tham quan!");
            }
            else if (check != null)
            {
                return BadRequest("Khong thể xóa");
            }          
            db.DiemThamQuans.Remove(DDCheck);
            db.SaveChanges();
            return Ok(input);
        }
    }
}
