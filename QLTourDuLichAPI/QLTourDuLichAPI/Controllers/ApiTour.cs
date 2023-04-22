using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using QLTourDuLichAPI.InputModelsApi;
using QLTourDuLichAPI.Models;
namespace QLTourDuLichAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiTour : Controller
    {

        QltourdlApiContext db = new QltourdlApiContext();
        [HttpGet]
        public IActionResult getAllTour()
        {
            var query = (from a in db.Tours                         
                         select new
                         {
                            a.MaTour,
                            a.TenTour,
                            a.Anh,
                            a.SoNgayDl,
                            a.XepHangTour,
                            a.GiaCho,
                            a.DiemXuatPhat,
                         });
            var totalCount = query.Count();
            //var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var listTour= query
                .ToList();

            var result = new
            {
                TotalCount = totalCount,
                //PageCount = pageCount,
                Items = listTour
            };

            return Ok(result);

        }
        [HttpGet]
        [Route("getPagination")]
        public IActionResult GetAllTourPagination([Range(1, 100)] int pageSize,
           [Range(1, int.MaxValue)] int pageNumber)
        {
            var listTour = (from a in db.Tours
                           select new
                           {
                               a.MaTour,
                               a.TenTour,
                               a.Anh,
                               a.SoNgayDl,
                               a.XepHangTour,
                               a.GiaCho,
                               a.DiemXuatPhat,
                           })
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
            var result = new
            {
                Items = listTour
            };
            return Ok(result);
        }
        [Route("getById")]
        [HttpGet]
        public IActionResult GetTourId(string id)
        {
            var Tour = (from a in db.Tours
                       where a.MaTour== id    
                          select new
                          {
                              a.MaTour,
                              a.TenTour,
                              a.Anh,
                              a.SoNgayDl,
                              a.XepHangTour,
                              a.GiaCho,
                              a.DiemXuatPhat,
                          })
                              .FirstOrDefault();
            return Ok(Tour);
        }
        [HttpPost]
        [Route("themTour")]
        public async Task<IActionResult> AddTour([FromForm] TourCreateInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Upload the image to the server
            string fileName = await UploadImage(input.Anh);
            var TourCheck = db.Tours.Select(x => x.MaTour).ToList();
            if (TourCheck.Any(x => x.Contains(input.MaTour)))
            {
                return BadRequest("Đã Tồn Tại Tour!");
            }

            var newTour = new Tour
            {
                MaTour = input.MaTour,
                TenTour = input.TenTour,
                GiaCho= input.GiaCho,
                XepHangTour= input.XepHangTour,
                Anh=fileName,
                SoNgayDl=input.SoNgayDl,
                DiemXuatPhat=input.DiemXuatPhat
            };

            db.Tours.Add(newTour);
            db.SaveChanges();
            return Ok(input);
        }
       
        [HttpPut]
        [Route("capnhatTour")]
        public async Task<IActionResult> UpdateTourAsync([FromForm] TourCreateInputModel input)
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
            var Tour = await db.Tours.FindAsync(input.MaTour);
            if (Tour == null)
            {
                return NotFound();
            }

            // Update the TinTuc object with the form data
            Tour.MaTour = input.MaTour;
            Tour.TenTour= input.TenTour;
            Tour.GiaCho = input.GiaCho;
            Tour.XepHangTour=input.XepHangTour;
            Tour.SoNgayDl= input.SoNgayDl;
          

            // Upload the image to the server and update the TinTuc object with the new image name
            if (input.Anh != null)
            {
                string fileName = await UploadImage(input.Anh);
                Tour.Anh = fileName;
            }

            // Update the TinTuc in the database
            db.Tours.Update(Tour);
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
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "anhdaidien", fileName);
            // Save the file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
        [HttpDelete]
        public IActionResult DeleteTour(string input)
        {
            var TourCheck = (from a in db.Tours
                           where a.MaTour == input
                           select a).FirstOrDefault();
                               
            if (TourCheck == null)
            {
                return BadRequest("Khong tim thay Tour!");
            }

            db.Tours.Remove(TourCheck);
            db.SaveChanges();

            return Ok(input);
        }
    }
}
