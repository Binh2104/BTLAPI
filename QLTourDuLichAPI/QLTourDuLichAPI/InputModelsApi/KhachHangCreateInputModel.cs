using QLTourDuLichAPI.Models;
namespace QLTourDuLichAPI.InputModelsApi
{
    public class KhachHangCreateInputModel
    {
        public string MaKH { get; set; } = null!;
        public string TenKH { get; set; } = null!;
        public string GioiTinh { get; set; } = null!;

        public string Sdt { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
