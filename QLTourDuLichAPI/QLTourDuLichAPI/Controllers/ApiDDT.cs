using Microsoft.AspNetCore.Mvc;
using QLTourDuLichAPI.InputModelsApi;
using QLTourDuLichAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace QLTourDuLichAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiDDT : Controller
    {
        QltourdlApiContext db = new QltourdlApiContext();
        [HttpGet]
        public IActionResult getAllDDT()
        {
            var query = (from a in db.DiaDiemTours 
                         join b in db.Tours on a.MaTour equals b.MaTour 
                         join c in db.DiemThamQuans on a.MaDd equals c.MaDd
                         orderby a.MaTour
                         select new
                         {
                             a.MaDd, 
                             a.MaTour,
                             b.TenTour,
                             c.Tendiadiem,
                             a.ViTriAnh,
                         });
            var totalCount = query.Count();
            //var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var listDDT = query
                .ToList();

            var result = new
            {
                TotalCount = totalCount,
                //PageCount = pageCount,
                Items = listDDT
            };

            return Ok(result);

        }
        [HttpGet]
        [Route("getPagination")]
        public IActionResult GetAllDDTPagination([Range(1, 100)] int pageSize,
         [Range(1, int.MaxValue)] int pageNumber)
        {
            var listDDT = (from a in db.DiaDiemTours
                           join b in db.Tours on a.MaTour equals b.MaTour
                           join c in db.DiemThamQuans on a.MaDd equals c.MaDd
                           orderby a.MaTour
                           select new
                           {
                               a.MaDd,
                               a.MaTour,
                               b.TenTour,
                               c.Tendiadiem,
                               a.ViTriAnh,
                           })
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
            var result = new
            {
                Items = listDDT
            };
            return Ok(result);
        }
        [Route("getById")]
        [HttpGet]
        public IActionResult GetDDTId(string id1, string id2)
        {
            var DDT = (from a in db.DiaDiemTours
                       join b in db.Tours on a.MaTour equals b.MaTour
                       join c in db.DiemThamQuans on a.MaDd equals c.MaDd
                       where a.MaTour == id1 && a.MaDd== id2                       
                       select new
                       {
                           a.MaDd,
                           a.MaTour,
                           b.TenTour,
                           c.Tendiadiem,
                           a.ViTriAnh,
                       })
                              .FirstOrDefault();
            return Ok(DDT);
        }

        [HttpPost]
        [Route("themDDT")]
        public async Task<IActionResult> AddDDT([FromForm] DiaDiemTourCreateInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Upload the image to the server
            /*var DDCheck = (from a in db.DiaDiemTours
                           where a.MaDd == input.MaDd && a.MaTour == input.MaTour
                           select a).ToList();
            if (DDCheck != null)
            {
                return BadRequest("Đã Tồn Tại !");
            }*/

            var newDDT = new DiaDiemTour
            {
                MaDd = input.MaDd,
                MaTour= input.MaTour,
                ViTriAnh= input.ViTriAnh,
                
            };

            db.DiaDiemTours.Add(newDDT);
            db.SaveChanges();
            return Ok(input);
        }
       /* [HttpPut]
        [Route("capnhatDDT")]
        public async Task<IActionResult> UpdateDTQAsync([FromForm] DiaDiemTourCreateInputModel input)
        {
            ViewBag.Username = HttpContext.Session.GetString("UserName");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the DiemThamQuan in the database by id
            var DTQ = await db.DiaDiemTours.FindAsync(input.MaTour);
            if (DTQ == null)
            {
                return NotFound();
            }
            DTQ.MaTour = input.MaTour;
            DTQ.MaDd = input.MaDd;
            DTQ.ViTriAnh = input.ViTriAnh; 
            
            db.DiaDiemTours.Update(DTQ);
            await db.SaveChangesAsync();
            return Ok();
        }*/

        [HttpDelete]
        [Route("xoa")]
        public IActionResult DeleteRecord(string matour, string madd)
        {
            var tour = db.Tours.FirstOrDefault(x => x.MaTour == matour);
            var dtq = db.DiemThamQuans.FirstOrDefault(x => x.MaDd == madd);

            if (tour == null || dtq == null)
            {
                return BadRequest("Tour hoặc điểm thăm quan không tồn tại.");
            }

            var recordtourdtq = db.DiaDiemTours.FirstOrDefault(x => x.MaTour == matour && x.MaDd == madd);

            if (recordtourdtq == null)
            {
                return BadRequest("Không tìm thấy bản ghi để xóa.");
            }

            db.DiaDiemTours.Remove(recordtourdtq);
            db.SaveChanges();

            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
