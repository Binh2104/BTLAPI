using QLTourDuLichAPI.Models;
namespace QLTourDuLichAPI.InputModelsApi
{
    public class TourCreateInputModel
    {
        public string MaTour { get; set; } = null!;

        public string TenTour { get; set; } = null!;

        public int GiaCho { get; set; }

        public IFormFile? Anh { get; set; } = null!;

        public string DiemXuatPhat { get; set; } = null!;

        public int XepHangTour { get; set; }

        public int SoNgayDl { get; set; }
    }
}
