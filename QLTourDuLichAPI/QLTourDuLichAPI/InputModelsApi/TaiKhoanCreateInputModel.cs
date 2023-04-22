using QLTourDuLichAPI.Models;
namespace QLTourDuLichAPI.InputModelsApi
{
    public class TaiKhoanCreateInputModel
    {
        public string UserName { get; set; } = null!;
        public int Loai { get; set; } =0;
        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;
    }
}
