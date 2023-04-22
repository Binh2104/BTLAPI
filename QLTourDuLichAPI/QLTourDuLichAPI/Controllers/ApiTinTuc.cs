using Microsoft.AspNetCore.Mvc;
using QLTourDuLichAPI.InputModelsApi;
using QLTourDuLichAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace QLTourDuLichAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class APITinTuc : Controller
    {

        QltourdlApiContext db = new QltourdlApiContext();
        [HttpGet]
        public IActionResult getAllTinTuc()
        {
            var x = (from a in db.TinTucs
                     select new
                     {
                         a.MaTin,
                         a.MaNv,
                         a.Anh,
                         a.MoTa,
                         a.NoiDung
                     });
            var totalCount = x.Count();
            var listTT = x
                .ToList();

            var result = new
            {
                TotalCount = totalCount,
                //PageCount = pageCount,
                Items = listTT
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("getPagination")]
        public IActionResult GetAllTintucPagination([Range(1, 100)] int pageSize,
           [Range(1, int.MaxValue)] int pageNumber)
        {
            var listTT = (from a in db.TinTucs
                          select new
                          {
                              a.MaTin,
                              a.MaNv,
                              a.Anh,
                              a.MoTa,
                              a.NoiDung
                          })
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
            var result = new
            {
                Items = listTT
            };
            return Ok(result);
        }
        [Route("getById")]
        [HttpGet]
        public IActionResult GetTintucId(string id)
        {
            var TT = (from a in db.TinTucs
                      where a.MaTin == id
                      select new
                      {
                          a.MaTin,
                          a.MaNv,
                          a.Anh,
                          a.MoTa,
                          a.NoiDung
                      }) .FirstOrDefault();
            return Ok(TT);
        }
        [HttpPost]
        [Route("themTT")]
        public async Task<IActionResult> AddTT([FromForm] TinTucCreateInputMode input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Upload the image to the server
            string fileName = await UploadImage(input.Anh);
            var DDCheck = db.TinTucs.Select(x => x.MaTin).ToList();
            if (DDCheck.Any(x => x.Contains(input.MaTin)))
            {
                return BadRequest("Đã Tồn Tại tin tuc!");
            }

            var newTT = new TinTuc
            {
                MaTin = input.MaTin,
                MaNv = input.MaNv,
                Anh = fileName,
                MoTa = input.MoTa,
                NoiDung = input.NoiDung

            };

            db.TinTucs.Add(newTT);
            db.SaveChanges();
            return Ok(input);
        }

        [HttpPut]
        [Route("capnhatTT")]
        public async Task<IActionResult> UpdateTTAsync([FromForm] TinTucCreateInputMode input)
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
            var TT = await db.TinTucs.FindAsync(input.MaTin);
            if (TT == null)
            {
                return NotFound();
            }

            // Update the TinTuc object with the form data
            TT.MaTin = input.MaTin;
            TT.MaNv = input.MaNv;
            TT.MoTa = input.MoTa;
            TT.NoiDung = input.NoiDung;


            // Upload the image to the server and update the TinTuc object with the new image name
            if (input.Anh != null)
            {
                string fileName = await UploadImage(input.Anh);
                TT.Anh = fileName;
            }

            // Update the TinTuc in the database
            db.TinTucs.Update(TT);
            await db.SaveChangesAsync();

            return Ok();
        }

        private Task<string> UploadImage(string anh)
        {
            throw new NotImplementedException();
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
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "AnhTinTuc", fileName);
            // Save the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        [HttpDelete]
        public IActionResult DeleteTT(string input)
        {
            var DDCheck = (from a in db.TinTucs
                           where a.MaTin == input
                           select a).FirstOrDefault();
            if (DDCheck == null)
            {
                return BadRequest("Khong tim thay tin tuc!");
            }
            var list = db.AnhTins.Where(x => x.MaTin == input);
            if (list != null) db.RemoveRange(list);
            db.TinTucs.Remove(DDCheck);
            db.SaveChanges();

            return Ok(input);
        }
    }
}
