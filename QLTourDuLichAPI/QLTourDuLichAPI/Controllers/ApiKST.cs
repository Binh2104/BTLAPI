using Microsoft.AspNetCore.Mvc;
using QLTourDuLichAPI.InputModelsApi;
using QLTourDuLichAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace QLTourDuLichAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKST : Controller
    {
        QltourdlApiContext db = new QltourdlApiContext();
        [HttpGet]
        public IActionResult getAllKST()
        {
            var query = (from a in db.KhachSans
                         join b in db.KhachSanTours on a.MaKs equals b.MaKs
                         join c in db.Tours on b.MaTour equals c.MaTour
                         orderby b.MaTour
                         select new
                         {
                             b.MaTour,
                             b.MaKs,
                             b.ViTriAnh,
                             a.TenKs,
                             c.TenTour,
                         });
            var totalCount = query.Count();
            //var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var listKST = query
                .ToList();

            var result = new
            {
                TotalCount = totalCount,
                //PageCount = pageCount,
                Items = listKST
            };

            return Ok(result);

        }
        [HttpGet]
        [Route("getPagination")]
        public IActionResult GetAllKSTPagination([Range(1, 100)] int pageSize,
         [Range(1, int.MaxValue)] int pageNumber)
        {
            var listKST = (from a in db.KhachSans
                           join b in db.KhachSanTours on a.MaKs equals b.MaKs
                           join c in db.Tours on b.MaTour equals c.MaTour
                           orderby b.MaTour
                           select new
                           {
                               b.MaTour,
                               b.MaKs,
                               b.ViTriAnh,
                               a.TenKs,
                               c.TenTour,
                           })
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
            var result = new
            {
                Items = listKST
            };
            return Ok(result);
        }
        [Route("getById")]
        [HttpGet]
        public IActionResult GetKSTId(string id1, string id2)
        {
            var KST = (from a in db.KhachSans
                       join b in db.KhachSanTours on a.MaKs equals b.MaKs
                       join c in db.Tours on b.MaTour equals c.MaTour
                       orderby b.MaTour
                       select new
                       {
                           b.MaTour,
                           b.MaKs,
                           b.ViTriAnh,
                           a.TenKs,
                           c.TenTour,
                       }).FirstOrDefault();
            return Ok(KST);
        }

        [HttpPost]
        [Route("themKST")]
        public async Task<IActionResult> AddKST([FromForm] KhachSanTourCreateInputMode input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newKST = new KhachSanTour
            {
                MaKs = input.MaKs,
                MaTour = input.MaTour,
                ViTriAnh = input.ViTriAnh,

            };

            db.KhachSanTours.Add(newKST);
            db.SaveChanges();
            return Ok(input);
        }

        [HttpDelete]
        [Route("xoa")]
        public IActionResult DeleteRecord(string matour, string maks)
        {
            var tour = db.Tours.FirstOrDefault(x => x.MaTour == matour);
            var dtq = db.KhachSans.FirstOrDefault(x => x.MaKs == maks);

            if (tour == null || dtq == null)
            {
                return BadRequest("Tour hoặc Khách sạn không tồn tại.");
            }

            var recordtourks = db.KhachSanTours.FirstOrDefault(x => x.MaTour == matour && x.MaKs ==maks);

            if (recordtourks == null)
            {
                return BadRequest("Không tìm thấy bản ghi để xóa.");
            }

            db.KhachSanTours.Remove(recordtourks);
            db.SaveChanges();

            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
